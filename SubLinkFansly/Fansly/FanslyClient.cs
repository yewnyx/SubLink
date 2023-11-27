using Serilog;
using SuperSocket.ClientEngine;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Timers;
using System.Threading.Tasks;
using WebSocket4Net;
using xyz.yewnyx.SubLink.Fansly.EventTypes;
using System.Linq;

namespace xyz.yewnyx.SubLink.Fansly;

internal sealed class FanslyClient {
    private const string _socketUri = "wss://chatws.fansly.com/";
    private const string _accountUri = "https://apiv3.fansly.com/api/v1/account?usernames={0}&ngsw-bypass=true";
    private const string _streamUri = "https://apiv3.fansly.com/api/v1/streaming/channel/{0}?ngsw-bypass=true";
    private const string _origin = "https://fansly.com";
    private const string _userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/114.0.0.0 Safari/537.36";

    private ILogger _logger;
    private readonly WebSocket _socket;
    private SocketAuth _token = new();
    private string _username = string.Empty;
    private SocketChatroom _chatroom = new();

    private readonly Timer _timer = new(2e4) { Enabled = false, AutoReset = true };

    //public event EventHandler<TipEventArgs>? TipEvent;

    public FanslyClient(ILogger logger) {
        _logger = logger;
        _timer.Elapsed += OnTimer;

        _socket = new(_socketUri, version: WebSocketVersion.Rfc6455, userAgent: _userAgent, origin: _origin) {
            EnableAutoSendPing = false,
            NoDelay = true
        };
        _socket.Opened += OnSockConnected;
        _socket.Error += OnSockError;
        _socket.Closed += OnSockDisconnected;
        _socket.MessageReceived += OnSockMessageReceived;
        _socket.DataReceived += OnSockDataReceived;
    }

    private void OnTimer(object? sender, ElapsedEventArgs e) =>
        _socket.Send("p");

    private void OnSockConnected(object? sender, EventArgs e) {
        _logger.Information("[{TAG}] Connected to socket", "Fansly");
        _socket.Send(_token.ToSocketMsg());
    }

    private void OnSockError(object? sender, ErrorEventArgs e) =>
        _logger.Error("[{TAG}] Error from socket : {ERR}", "Fansly", e.Exception);

    private void OnSockDisconnected(object? sender, EventArgs e) =>
        _logger.Information("[{TAG}] Disconnected from socket", "Fansly");

    private void OnSockMessageReceived(object? sender, MessageReceivedEventArgs e) {
        SocketMsg msg = JsonSerializer.Deserialize<SocketMsg>(e.Message) ?? new();

        switch (msg.Type) {
            case SocketMessageType.Auth: {
                _logger.Information($"[{{TAG}}] Joining chatroom {_chatroom.ChatRoomId}", "Fansly");
                _socket.Send(_chatroom.ToSocketMsg());
                break;
            }
            case SocketMessageType.Service: {
                SocketServiceMsg ssMsg = JsonSerializer.Deserialize<SocketServiceMsg>(msg.Data) ?? new();
                BaseEventType eventType = JsonSerializer.Deserialize<BaseEventType>(ssMsg.Event) ?? new();

                switch (eventType.Type) {
                    case EventType.ChatRoomMessage: {
                        ChatRoomMessage message = JsonSerializer.Deserialize<ChatRoomMessage>(ssMsg.Event) ?? new();
                        var attachment = message.Data.Attachments.Where(
                            item => item.ContentType == AttachmentContentType.Tip
                        );
                        TipMessageMetadata? tip = attachment.Any()
                            ? JsonSerializer.Deserialize<TipMessageMetadata>(attachment.Single().Metadata)
                            : null;

                        _logger.Information(
                            "[{TAG}] Chat message received: {Displayname} > {Content} ; Tip: {TipAmount}", "Fansly",
                            message.Data.Displayname,
                            message.Data.Content,
                            tip?.Amount ?? 0
                        );
                        break;
                    }
                    case EventType.ChatRoomGoal: {
                        ChatRoomGoal goal = JsonSerializer.Deserialize<ChatRoomGoal>(ssMsg.Event) ?? new();
                        _logger.Information(
                            "[{TAG}] Goal updated: `{Label}` {CurrentAmount} / {GoalAmount}", "Fansly",
                            goal.Data.Label,
                            goal.Data.CurrentAmount,
                            goal.Data.GoalAmount
                        );
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

    private void OnSockDataReceived(object? sender, DataReceivedEventArgs e) {
        _logger.Information("[{TAG}] Data received, length: {Length}", "Fansly", e.Data.Length);
    }

    public async Task<bool> ConnectAsync(string token, string username) {
        _token.Token = token;
        _username = username;

        try {
            string accountJsonStr = string.Empty;
            string streamJsonStr = string.Empty;
            AccountInfoResponse account;
            StreamInfoResponse stream;

            using (HttpClient client = new()) {
                using (HttpRequestMessage request = new(HttpMethod.Get, string.Format(_accountUri, _username))) {
                    request.Headers.Add("authority", "apiv3.fansly.com");
                    request.Headers.Add("Accept", "application/json, text/plain, */*");
                    request.Headers.Add("accept-language", "en;q=0.8,en-US;q=0.7");
                    request.Headers.Add("authorization", _token.Token);
                    request.Headers.Add("origin", _origin);
                    request.Headers.Add("referer", _origin);
                    request.Headers.Add("sec-ch-ua", "Not.A/Brand\";v=\"8\", \"Chromium\";v=\"114\", \"Google Chrome\";v=\"114\"");
                    request.Headers.Add("sec-ch-ua-mobile", "?0");
                    request.Headers.Add("sec-ch-ua-platform", "\"Windows\"");
                    request.Headers.Add("sec-fetch-dest", "empty");
                    request.Headers.Add("sec-fetch-mode", "cors");
                    request.Headers.Add("sec-fetch-site", "same-site");
                    request.Headers.Add("user-agent", _userAgent);
                    var response = await client.SendAsync(request);
                    accountJsonStr = await response.Content.ReadAsStringAsync();
                    account = JsonSerializer.Deserialize<AccountInfoResponse>(accountJsonStr) ?? new();
                }

                if (!(account.Success && account.Response != null && account.Response.Length > 0)) {
                    _logger.Error("[{TAG}] Invalid API response", "Fansly");
                    return false;
                }

                using (HttpRequestMessage request = new(HttpMethod.Get, string.Format(_streamUri, account.Response[0].Id))) {
                    request.Headers.Add("authority", "apiv3.fansly.com");
                    request.Headers.Add("Accept", "application/json, text/plain, */*");
                    request.Headers.Add("accept-language", "en;q=0.8,en-US;q=0.7");
                    request.Headers.Add("authorization", _token.Token);
                    request.Headers.Add("origin", _origin);
                    request.Headers.Add("referer", _origin);
                    request.Headers.Add("sec-ch-ua", "Not.A/Brand\";v=\"8\", \"Chromium\";v=\"114\", \"Google Chrome\";v=\"114\"");
                    request.Headers.Add("sec-ch-ua-mobile", "?0");
                    request.Headers.Add("sec-ch-ua-platform", "\"Windows\"");
                    request.Headers.Add("sec-fetch-dest", "empty");
                    request.Headers.Add("sec-fetch-mode", "cors");
                    request.Headers.Add("sec-fetch-site", "same-site");
                    request.Headers.Add("user-agent", _userAgent);
                    var response = await client.SendAsync(request);
                    streamJsonStr = await response.Content.ReadAsStringAsync();
                    stream = JsonSerializer.Deserialize<StreamInfoResponse>(streamJsonStr) ?? new();
                }

                if (!(stream.Success && stream.Response != null)) {
                    _logger.Error("[{TAG}] Stream offline", "Fansly");
                    return false;
                }

                _chatroom.ChatRoomId = stream.Response.ChatRoomId;
            }

            await _socket.OpenAsync();
            _timer.Start();
            return true;
        } catch (Exception ex) {
            _logger.Error("[{TAG}] Error while connecting:\r\n{ERR}", "Fansly", ex);
            return false;
        }
    }

    public async Task DisconnectAsync() {
        _timer.Stop();
        await _socket.CloseAsync();
    }
}
