using Serilog;
using SocketIOClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace xyz.yewnyx.SubLink.StreamElements.SEClient;

internal sealed class StreamElementsClient {
    private const string _socketUri = "wss://realtime.streamelements.com";

    private readonly ILogger _logger;
    private readonly SocketIOClient.SocketIO _socket;
    private string _token = string.Empty;

    public event EventHandler<TipEventArgs>? TipEvent;

    public StreamElementsClient(ILogger logger) {
        _logger = logger;

        _socket = new(_socketUri, new() {
            RandomizationFactor = 0.5,
            ReconnectionDelay = 500.0,
            ReconnectionDelayMax = 1000,
            ReconnectionAttempts = 5,
            Path = "/socket.io/",
            ConnectionTimeout = TimeSpan.FromSeconds(30),
            Reconnection = true,
            Transport = SocketIOClient.Transport.TransportProtocol.WebSocket,
            EIO = SocketIO.Core.EngineIO.V3,
            AutoUpgrade = true,
            Query = new KeyValuePair<string, string>[] { new("cluster", "main") },
        });

        _socket.OnConnected += OnConnected;
        _socket.OnDisconnected += OnDisconnected;
        _socket.OnError += OnError;
        _socket.OnReconnectAttempt += OnReconnectAttempt;
        _socket.OnReconnected += OnReconnected;
        _socket.OnReconnectError += OnReconnectError;
        _socket.OnReconnectFailed += OnReconnectFailed;
        _socket.On("authenticated", OnAuthenticated);
        _socket.On("unauthorized", OnUnauthorized);
        _socket.On("event", OnEvent);
    }

    private async void OnConnected(object? sender, EventArgs e) {
        _logger.Information("[{TAG}] Connected", Platform.PlatformName);
        await _socket.EmitAsync("authenticate", new SocketAuth("jwt", _token));
    }

    private void OnDisconnected(object? sender, string e) =>
        _logger.Information("[{TAG}] Disconnected", Platform.PlatformName);

    private void OnError(object? sender, string e) =>
        _logger.Error("[{TAG}] Error: {ERROR}", Platform.PlatformName, e);

    private void OnReconnectAttempt(object? sender, int e) =>
        _logger.Debug("[{TAG}] Socket reconnect attempt #{e}", Platform.PlatformName, e);

    private void OnReconnected(object? sender, int e) =>
        _logger.Information("[{TAG}] Socket reconnected after {e} attempts", Platform.PlatformName, e);

    private void OnReconnectError(object? sender, Exception e) =>
        _logger.Error("[{TAG}] Socket reconnect error:", Platform.PlatformName, e);

    private void OnReconnectFailed(object? sender, EventArgs e) =>
        _logger.Error("[{TAG}] Socket reconnect failed", Platform.PlatformName);

    private void OnAuthenticated(SocketIOResponse response) =>
        _logger.Information("[{TAG}] Authenticated", Platform.PlatformName);

    private void OnUnauthorized(SocketIOResponse response) =>
        _logger.Information("[{TAG}] Not authorized to use the Realtime API", Platform.PlatformName);

    private void OnEvent(SocketIOResponse response) {
        SocketEvent sockEvent = response.GetValue<SocketEvent>();

        if (sockEvent == null) {
            _logger.Error("[{TAG}] Invalid event data recieved", Platform.PlatformName);
            return;
        }

        switch (sockEvent.Type) {
            case "tip": {
                float amount = Convert.ToSingle(sockEvent.Data["amount"]);
                TipEvent?.Invoke(this, new() {
                    Name = (string?)sockEvent.Data["username"] ?? string.Empty,
                    Amount = amount,
                    CentAmount = (int)MathF.Round(amount * 100, 0, MidpointRounding.ToZero),
                    Message = (string?)sockEvent.Data["message"] ?? string.Empty,
                    UserCurrency = (string?)sockEvent.Data["currency"] ?? string.Empty
                });
                break;
            }
            default: {
                _logger.Debug("[{TAG}] Ignoring unsupported event of type: {TYPE}", Platform.PlatformName, sockEvent.Type);
                break;
            }
        }
    }

    public async Task<bool> ConnectAsync(string token) {
        _token = token;

        try {
            await _socket.ConnectAsync();
            return true;
        } catch (Exception) {
            return false;
        }
    }

    public async Task DisconnectAsync() =>
        await _socket.DisconnectAsync();
}
