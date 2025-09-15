using Serilog;
using SocketIOClient;
using System;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using TwitchLib.Api.Core.Interfaces;

namespace xyz.yewnyx.SubLink.Discord.Client;

internal class DiscordClient : IDisposable
{
    private NamedPipeClientStream? _pipeClient;
    private readonly object _writeLock = new();
    private readonly object _readLock = new();
    private readonly object _pipeLock = new();
    private CancellationTokenSource? _cts;
    private string _pipeName = string.Empty;
    private readonly ILogger _logger;

    public event EventHandler? OnReady;
    public event EventHandler<DiscordErrorArgs>? OnError;
    public event EventHandler<DiscordVoiceChannelIdEventArgs>? OnSelectedVoiceChannel;
    public event EventHandler<DiscordVoiceSettingsEventArgs>? OnVoiceSettingsUpdate;
    public event EventHandler<DiscordVoiceStatusEventArgs>? OnVoiceStatusUpdate;
    public event EventHandler<DiscordGuildIdEventArgs>? OnGuildStatus;
    public event EventHandler<DiscordGuildIdEventArgs>? OnGuildCreate;
    public event EventHandler<DiscordChannelIdEventArgs>? OnChannelCreate;
    public event EventHandler<DiscordUserIdEventArgs>? OnVoiceStateCreate;
    public event EventHandler<DiscordUserIdEventArgs>? OnVoiceStateUpdate;
    public event EventHandler<DiscordUserIdEventArgs>? OnVoiceStateDelete;
    public event EventHandler<DiscordUserIdEventArgs>? OnStartSpeaking;
    public event EventHandler<DiscordUserIdEventArgs>? OnStopSpeaking;
    public event EventHandler<DiscordMessageIdEventArgs>? OnMessageCreate;
    public event EventHandler<DiscordMessageIdEventArgs>? OnMessageUpdate;
    public event EventHandler<DiscordMessageIdEventArgs>? OnMessageDelete;
    public event EventHandler<DiscordChannelIdEventArgs>? OnNotificationCreate;
    public event EventHandler? OnActivityJoin;
    public event EventHandler? OnActivitySpectate;
    public event EventHandler<DiscordUserIdEventArgs>? OnActivityJoinRequest;

    public DiscordClient(ILogger logger)
    {
        _logger = logger;
    }

    public void Dispose()
    {
        StopListening();
        _pipeClient?.Dispose();
        _pipeClient = null;
    }

    private void IntConnect()
    {
        _pipeClient = new NamedPipeClientStream(
            serverName: ".",
            pipeName: _pipeName,
            direction: PipeDirection.InOut,
            options: PipeOptions.Asynchronous
        );
        _pipeClient.Connect(timeout: 1000);
    }

    public void Connect(string pipeName)
    {
        _pipeName = pipeName;
        IntConnect();
    }

    private void Reconnect()
    {
        Dispose();
        IntConnect();
    }

    public void StartListening()
    {
        if (_pipeClient == null || !_pipeClient.IsConnected)
            throw new InvalidOperationException("Must call Connect(...) before listening.");

        _cts = new CancellationTokenSource();
        Task.Run(() =>
        {
            var header = new byte[8];
            try
            {
                while (!_cts.Token.IsCancellationRequested)
                {
                    // 1) Read header
                    lock (_readLock)
                    {
                        int got = _pipeClient.Read(header, 0, 8);
                        if (got == 0) break; // pipe closed
                    }

                    // Extract length
                    int length = BitConverter.ToInt32(header, 4);
                    var body = new byte[length];

                    // 2) Read body
                    lock (_readLock)
                    {
                        int read = 0;

                        while (read < length)
                            read += _pipeClient.Read(body, read, length - read);
                    }

                    // 3) Deserialize + dispatch
                    var evt = JsonSerializer.Deserialize<JsonElement>(Encoding.UTF8.GetString(body));
                    Task.Run(() => HandleRpcEvent(evt));
                }
            }
            catch (IOException ex)
            {
                _logger.Error("[{TAG}] Error in pipe IO: {ERROR}", Platform.PlatformName, ex.Message);
            }
        }, _cts.Token);
    }

    public void StopListening() =>
        _cts?.Cancel();

    public string SendDataAndWait(int op, object payload)
    {
        if (_pipeClient == null || !_pipeClient.IsConnected)
            throw new InvalidOperationException("You must connect your client before sending events!");

        try
        {
            // Serialize payload to JSON
            string payloadJson = JsonSerializer.Serialize(payload);
            byte[] payloadBytes = Encoding.UTF8.GetBytes(payloadJson);

            // Create a header with operation code and payload length
            byte[] header = BitConverter.GetBytes(op)
                .Concat(BitConverter.GetBytes(payloadBytes.Length))
                .ToArray();

            string responseJson = string.Empty;
            lock (_pipeLock)
            {
                // Write header and payload to the pipe
                _pipeClient.Write(header, 0, header.Length);
                _pipeClient.Write(payloadBytes, 0, payloadBytes.Length);
                _pipeClient.Flush();
                _logger.Debug("[{TAG}] Payload sent: {PAYLOAD}", Platform.PlatformName, payloadJson);

                // Read the response
                byte[] responseHeader = new byte[8];
                _pipeClient.Read(responseHeader, 0, 8);
                int statusCode = BitConverter.ToInt32(responseHeader, 0);
                int responseLength = BitConverter.ToInt32(responseHeader, 4);

                byte[] responseBytes = new byte[responseLength];
                _pipeClient.Read(responseBytes, 0, responseLength);
                _logger.Debug("[{TAG}] Response received: {RESPONSE}", Platform.PlatformName, responseJson);
                return responseJson;
            }
        }
        catch (IOException ex)
        {
            _logger.Error("[{TAG}] Pipe communication error: {ERROR}", Platform.PlatformName, ex.Message);

            if (ex.Message.Contains("broken"))
            {
                _logger.Debug("[{TAG}] Attempting to reconnect...", Platform.PlatformName);
                Reconnect();
                return SendDataAndWait(op, payload); // Retry sending the payload
            }

            throw;
        }
    }

    public void SendCommand(int op, object payload)
    {
        if (_pipeClient == null || !_pipeClient.IsConnected)
            throw new InvalidOperationException("Must call Connect(...) before sending commands.");

        // 1) Serialize payload
        string json = JsonSerializer.Serialize(payload);
        byte[] body = Encoding.UTF8.GetBytes(json);

        // 2) Build 8-byte header: [ op (4 bytes) | length (4 bytes) ]
        byte[] header = BitConverter.GetBytes(op)
            .Concat(BitConverter.GetBytes(body.Length))
            .ToArray();

        // 3) Atomically write header + body
        lock (_writeLock)
        {
            _pipeClient.Write(header, 0, header.Length);
            _pipeClient.Write(body, 0, body.Length);
            _pipeClient.Flush();
        }
    }

    public string Handshake(string clientId) =>
        SendDataAndWait(0, new { v = 1, client_id = clientId });

    private static int VoiceStateToInt(string state) =>
        state switch
        {
            "DISCONNECTED" => 0,
            "AWAITING_ENDPOINT" => 1,
            "AUTHENTICATING" => 2,
            "CONNECTING" => 3,
            "CONNECTED" => 4,
            "VOICE_DISCONNECTED" => 5,
            "VOICE_CONNECTING" => 6,
            "VOICE_CONNECTED" => 7,
            "NO_ROUTE" => 8,
            "ICE_CHECKING" => 9,
            _ => -1
        };

    private static int EventNameToInt(string evtName) =>
        evtName switch
        {
            "READY" => 0,
            "ERROR" => 1,
            "GUILD_STATUS" => 2,
            "GUILD_CREATE" => 3,
            "CHANNEL_CREATE" => 4,
            "VOICE_CHANNEL_SELECT" => 5,
            "VOICE_STATE_CREATE" => 6,
            "VOICE_STATE_UPDATE" => 7,
            "VOICE_STATE_DELETE" => 8,
            "VOICE_SETTINGS_UPDATE" => 9,
            "VOICE_CONNECTION_STATUS" => 10,
            "SPEAKING_START" => 11,
            "SPEAKING_STOP" => 12,
            "MESSAGE_CREATE" => 13,
            "MESSAGE_UPDATE" => 14,
            "MESSAGE_DELETE" => 15,
            "NOTIFICATION_CREATE" => 16,
            "ACTIVITY_JOIN" => 17,
            "ACTIVITY_SPECTATE" => 18,
            "ACTIVITY_JOIN_REQUEST" => 19,
            _ => -1
        };

    private void HandleRpcEvent(JsonElement evt)
    {
        try
        {
            _logger.Debug("[{TAG}] Event received: {evt}", Platform.PlatformName, evt);
            if (!evt.TryGetProperty("evt", out var evtNameEl)) return;
            string evtName = evtNameEl.GetString() ?? string.Empty;
            int evtCode = EventNameToInt(evtName);

            switch (evtName)
            {
                case "READY":
                    {
                        OnReady?.Invoke(this, new());
                        break;
                    }
                case "ERROR":
                    {
                        if (evt.TryGetProperty("data", out var err) && err.TryGetProperty("code", out var codeEl))
                            OnError?.Invoke(this, new(codeEl.GetInt32()));

                        break;
                    }
                case "VOICE_CHANNEL_SELECT":
                    {
                        if (evt.TryGetProperty("data", out var vc) && vc.TryGetProperty("channel_id", out var cid))
                            OnSelectedVoiceChannel?.Invoke(this, new(cid.GetString() ?? string.Empty));

                        break;
                    }
                case "VOICE_SETTINGS_UPDATE":
                    {
                        if (evt.TryGetProperty("data", out var vs))
                        {
                            float inVol = vs.GetProperty("input").GetProperty("volume").GetSingle();
                            float outVol = vs.GetProperty("output").GetProperty("volume").GetSingle();
                            OnVoiceSettingsUpdate?.Invoke(this, new(inVol, outVol));
                        }

                        break;
                    }
                case "VOICE_CONNECTION_STATUS":
                    {
                        if (evt.TryGetProperty("data", out var vcstat) && vcstat.TryGetProperty("state", out var stateEl))
                        {
                            string state = stateEl.GetString() ?? string.Empty;
                            int stateCode = VoiceStateToInt(state);
                            OnVoiceStatusUpdate?.Invoke(this, new(state, stateCode));
                        }

                        break;
                    }
                case "GUILD_STATUS":
                    {
                        if (evt.TryGetProperty("data", out var gs) && gs.TryGetProperty("guild", out var g) && g.TryGetProperty("id", out var gidElStatus))
                            OnGuildStatus?.Invoke(this, new(gidElStatus.GetString() ?? string.Empty));

                        break;
                    }
                case "GUILD_CREATE":
                    {
                        if (evt.TryGetProperty("data", out var gc) && gc.TryGetProperty("id", out var gidc))
                            OnGuildCreate?.Invoke(this, new(gidc.GetString() ?? string.Empty));

                        break;
                    }
                case "CHANNEL_CREATE":
                    {
                        if (evt.TryGetProperty("data", out var ch) && ch.TryGetProperty("id", out var cidEl))
                            OnChannelCreate?.Invoke(this, new(cidEl.GetString() ?? string.Empty));

                        break;
                    }
                case "VOICE_STATE_CREATE":
                    {
                        if (evt.TryGetProperty("data", out var vsd) && vsd.TryGetProperty("user", out var usr) && usr.TryGetProperty("id", out var uid))
                            OnVoiceStateCreate?.Invoke(this, new(uid.GetString() ?? string.Empty));

                        break;
                    }
                case "VOICE_STATE_UPDATE":
                    {
                        if (evt.TryGetProperty("data", out var vsd) && vsd.TryGetProperty("user", out var usr) && usr.TryGetProperty("id", out var uid))
                            OnVoiceStateUpdate?.Invoke(this, new(uid.GetString() ?? string.Empty));

                        break;
                    }
                case "VOICE_STATE_DELETE":
                    {
                        if (evt.TryGetProperty("data", out var vsd) && vsd.TryGetProperty("user", out var usr) && usr.TryGetProperty("id", out var uid))
                            OnVoiceStateDelete?.Invoke(this, new(uid.GetString() ?? string.Empty));

                        break;
                    }
                case "SPEAKING_START":
                    {
                        if (evt.TryGetProperty("data", out var sp) && sp.TryGetProperty("user_id", out var suid))
                            OnStartSpeaking?.Invoke(this, new(suid.GetString() ?? string.Empty));

                        break;
                    }
                case "SPEAKING_STOP":
                    {
                        if (evt.TryGetProperty("data", out var sp) && sp.TryGetProperty("user_id", out var suid))
                            OnStopSpeaking?.Invoke(this, new(suid.GetString() ?? string.Empty));

                        break;
                    }
                case "MESSAGE_CREATE":
                    {
                        if (evt.TryGetProperty("data", out var msg) && msg.TryGetProperty("message", out var m) && m.TryGetProperty("id", out var mid))
                            OnMessageCreate?.Invoke(this, new(mid.GetString() ?? string.Empty));

                        break;
                    }
                case "MESSAGE_UPDATE":
                    {
                        if (evt.TryGetProperty("data", out var msg) && msg.TryGetProperty("message", out var m) && m.TryGetProperty("id", out var mid))
                            OnMessageUpdate?.Invoke(this, new(mid.GetString() ?? string.Empty));

                        break;
                    }
                case "MESSAGE_DELETE":
                    {
                        if (evt.TryGetProperty("data", out var msg) && msg.TryGetProperty("message", out var m) && m.TryGetProperty("id", out var mid))
                            OnMessageDelete?.Invoke(this, new(mid.GetString() ?? string.Empty));

                        break;
                    }
                case "NOTIFICATION_CREATE":
                    {
                        if (evt.TryGetProperty("data", out var noti) && noti.TryGetProperty("channel_id", out var nch))
                            OnNotificationCreate?.Invoke(this, new(nch.GetString() ?? string.Empty));

                        break;
                    }
                case "ACTIVITY_JOIN":
                    {
                        OnActivityJoin?.Invoke(this, new());
                        break;
                    }
                case "ACTIVITY_SPECTATE":
                    {
                        OnActivitySpectate?.Invoke(this, new());
                        break;
                    }
                case "ACTIVITY_JOIN_REQUEST":
                    {
                        if (evt.TryGetProperty("data", out var aj) && aj.TryGetProperty("user", out var aju) && aju.TryGetProperty("id", out var ajuId))
                            OnActivityJoinRequest?.Invoke(this, new(ajuId.GetString() ?? string.Empty));

                        break;
                    }
            }
        }
        catch (Exception ex)
        {
            _logger.Warning("[{TAG}] Failed to handle RPC event: {ERROR}", Platform.PlatformName, ex.Message);
        }
    }
}
