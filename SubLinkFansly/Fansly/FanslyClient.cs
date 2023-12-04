using Serilog;
using SuperSocket.ClientEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Timers;
using System.Threading.Tasks;
using WebSocket4Net;
using xyz.yewnyx.SubLink.Fansly.APIDataTypes;
using xyz.yewnyx.SubLink.Fansly.SocketDataTypes;

namespace xyz.yewnyx.SubLink.Fansly;

internal sealed class FanslyClient {
    private const string _socketUri = "wss://chatws.fansly.com/";
    private const string _accountUri = "https://apiv3.fansly.com/api/v1/account?usernames={0}&ngsw-bypass=true";
    private const string _streamUri = "https://apiv3.fansly.com/api/v1/streaming/channel/{0}?ngsw-bypass=true";
    private const string _origin = "https://fansly.com";
    private const string _userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/114.0.0.0 Safari/537.36";
    private readonly static Dictionary<string, string> _headers = new() {
        { "authority", "apiv3.fansly.com" },
        { "accept", "application/json, text/plain, */*" },
        { "accept-language", "en;q=0.8,en-US;q=0.7" },
        { "origin", _origin },
        { "referer", _origin },
        { "sec-ch-ua", "Not.A/Brand\";v=\"8\", \"Chromium\";v=\"114\", \"Google Chrome\";v=\"114\"" },
        { "sec-ch-ua-mobile", "?0" },
        { "sec-ch-ua-platform", "\"Windows\"" },
        { "sec-fetch-dest", "empty" },
        { "sec-fetch-mode", "cors" },
        { "sec-fetch-site", "same-site" },
        { "user-agent", _userAgent }
    };
    // Fansly's own socket specifies `2e4` as ping interval
    private readonly Timer _socketPingTimer = new(2e4) { Enabled = false, AutoReset = true };

    private readonly ILogger _logger;
    private readonly WebSocket _socket;
    private readonly SocketAuth _token = new();
    private SocketChatroom? _chatroom;

    public event EventHandler? FanslyConnected;

    public event EventHandler? FanslyDisconnected;

    public event EventHandler<FanslyErrorArgs>? FanslyError;

    public event EventHandler<ChatMessageEventArgs>? ChatMessageEvent;

    public event EventHandler<TipEventArgs>? TipEvent;

    public event EventHandler<GoalUpdatedEventArgs>? GoalUpdatedEvent;

    public FanslyClient(ILogger logger) {
        _logger = logger;
        _socketPingTimer.Elapsed += OnTimer;

        _socket = new(_socketUri, version: WebSocketVersion.Rfc6455, userAgent: _userAgent, origin: _origin) {
            EnableAutoSendPing = false,
            NoDelay = true
        };
        _socket.Opened += OnSockConnected;
        _socket.Closed += OnSockDisconnected;
        _socket.Error += OnSockError;
        _socket.MessageReceived += OnSockMessageReceived;
        _socket.DataReceived += OnSockDataReceived;
    }

    private void OnTimer(object? sender, ElapsedEventArgs e) =>
        _socket.Send("p");

    private void OnSockConnected(object? sender, EventArgs e) {
        FanslyConnected?.Invoke(this, e);
        _socket.Send(_token.ToSocketMsg());
    }

    private void OnSockDisconnected(object? sender, EventArgs e) =>
        FanslyDisconnected?.Invoke(this, e);

    private void OnSockError(object? sender, ErrorEventArgs e) =>
        FanslyError?.Invoke(this, new(e.Exception));

    private void OnSockMessageReceived(object? sender, MessageReceivedEventArgs e) {
        SocketMsg msg = JsonSerializer.Deserialize<SocketMsg>(e.Message) ?? new();

        switch (msg.Type) {
            case SocketMessageType.Auth: {
                if (_chatroom == null)
                    break;

                _logger.Information("[{TAG}] Joining chatroom {ChatRoomId}", "Fansly", _chatroom.ChatRoomId);
                _socket.Send(_chatroom.ToSocketMsg());
                break;
            }
            case SocketMessageType.Service: {
                SocketService ssMsg = JsonSerializer.Deserialize<SocketService>(msg.Data) ?? new();
                BaseEventType eventType = JsonSerializer.Deserialize<BaseEventType>(ssMsg.Event) ?? new();

                switch (eventType.Type) {
                    case EventType.ChatRoomMessage: {
                        ChatRoomMessage message = JsonSerializer.Deserialize<ChatRoomMessage>(ssMsg.Event) ?? new();
                        var attachment = message.Data.Attachments.Where(
                            item => item.ContentType == AttachmentContentType.Tip
                        );
                        TipAttachmentMetadata? tip = attachment.Any()
                            ? JsonSerializer.Deserialize<TipAttachmentMetadata>(attachment.Single().Metadata)
                            : null;

                        if (tip != null) {
                            TipEvent?.Invoke(this, new TipEventArgs { Data = new(
                                message.Data.Id,
                                message.Data.ChatRoomId,
                                message.Data.SenderId,
                                message.Data.Username,
                                message.Data.Displayname,
                                message.Data.Content,
                                tip.Amount,
                                message.Data.CreatedAt
                            ) });
                        } else {
                            ChatMessageEvent?.Invoke(this, new ChatMessageEventArgs { Data = new(
                                message.Data.Id,
                                message.Data.ChatRoomId,
                                message.Data.SenderId,
                                message.Data.Username,
                                message.Data.Displayname,
                                message.Data.Content,
                                message.Data.CreatedAt
                            ) });
                        }

                        break;
                    }
                    case EventType.ChatRoomGoal: {
                        ChatRoomGoal goal = JsonSerializer.Deserialize<ChatRoomGoal>(ssMsg.Event) ?? new();
                        GoalUpdatedEvent?.Invoke(this, new GoalUpdatedEventArgs { Data = new(
                            goal.Data.Id,
                            goal.Data.ChatRoomId,
                            goal.Data.AccountId,
                            goal.Data.Type,
                            goal.Data.Label,
                            goal.Data.Description,
                            goal.Data.Status,
                            (int)Math.Round(goal.Data.CurrentAmount / 10d, 0, MidpointRounding.ToZero),
                            (int)Math.Round(goal.Data.GoalAmount / 10d, 0, MidpointRounding.ToZero),
                            goal.Data.Version
                        ) });
                        break;
                    }
                    default: {
                        _logger.Information("[{TAG}] Chat message received: {Message}", "Fansly", msg.Data);
                        break;
                    }
                }
                break;
            }
            default: {
                _logger.Information("[{TAG}] Unknown socket message received: {Message}", "Fansly", e.Message);
                break;
            }
        }
    }

    private void OnSockDataReceived(object? sender, DataReceivedEventArgs e) =>
        _logger.Information("[{TAG}] Data received, length: {Length}", "Fansly", e.Data.Length);

    private static async Task<T?> GetFanslyApiData<T>(HttpClient client, string uri, params object?[] args)
        where T : BaseApiResponse, new()
    {
        using HttpRequestMessage request = new(HttpMethod.Get, string.Format(uri, args));

        foreach (var header in _headers) {
            request.Headers.Add(header.Key, header.Value);
        }

        HttpResponseMessage response = await client.SendAsync(request);
        string jsonStr = await response.Content.ReadAsStringAsync();
        T? result = JsonSerializer.Deserialize<T>(jsonStr);

        return result != null && result.Success
            ? result
            : null;
    }

    private async Task<SocketChatroom?> GetChatRoomIdAsync(string username) {
        try {
            string jsonStr = string.Empty;
            using HttpClient client = new();
            var account = await GetFanslyApiData<AccountInfoResponse>(client, _accountUri, username);

            if (account == null || account.Response.Length <= 0) {
                _logger.Error("[{TAG}] Invalid API response", "Fansly");
                return null;
            }

            var stream = await GetFanslyApiData<StreamInfoResponse>(client, _streamUri, account.Response[0].Id);

            if (stream == null || stream.Response == null) {
                _logger.Error("[{TAG}] Stream offline", "Fansly");
                return null;
            }

            return new(stream.Response.ChatRoomId);
        } catch (Exception ex) {
            _logger.Error("[{TAG}] Error while retrieving the chatroom ID:\r\n{ERR}", "Fansly", ex);
            return null;
        }
    }

    public async Task<bool> ConnectAsync(string token, string username) {
        _token.Token = token;
        _headers.Remove("authorization");
        _headers.Add("authorization", _token.Token);

        try {
            _chatroom = await GetChatRoomIdAsync(username);
            await _socket.OpenAsync();
            _socketPingTimer.Start();
            return true;
        } catch (Exception ex) {
            _logger.Error("[{TAG}] Error while connecting:\r\n{ERR}", "Fansly", ex);
            return false;
        }
    }

    public async Task DisconnectAsync() {
        _socketPingTimer.Stop();

        if (_socket.State != WebSocketState.Closed)
            await _socket.CloseAsync();
    }
}
