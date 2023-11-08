using Serilog;
using SocketIOClient;
using System;
using System.Threading.Tasks;

namespace xyz.yewnyx.SubLink.StreamElements;

internal sealed class StreamElementsClient {
    private const string _socketUri = "https://realtime.streamelements.com";

    private ILogger _logger;
    private readonly SocketIOClient.SocketIO _socket;
    private string _token = string.Empty;

    public event EventHandler<TipEventArgs>? TipEvent;

    public StreamElementsClient(ILogger logger) {
        _logger = logger;

        _socket = new(_socketUri);
        _socket.Options.AutoUpgrade = true;
        _socket.Options.ConnectionTimeout = TimeSpan.FromSeconds(5);
        _socket.Options.Reconnection = true;
        _socket.Options.ReconnectionAttempts = 3;
        _socket.Options.Transport = SocketIOClient.Transport.TransportProtocol.WebSocket;

        _socket.OnConnected += OnConnected;
        _socket.OnDisconnected += OnDisconnected;
        _socket.OnError += OnError;
        _socket.On("authenticated", OnAuthenticated);
        _socket.On("unauthorized", OnUnauthorized);
        _socket.On("event", OnEvent);
    }

    private async void OnConnected(object? sender, EventArgs e) {
        _logger.Information("[{TAG}] Connected to StreamElements", "StreamElements");
        await _socket.EmitAsync("authenticate", new SocketAuth("jwt", _token));
    }

    private void OnDisconnected(object? sender, string e) =>
        _logger.Information("[{TAG}] Disconnected from StreamElements", "StreamElements");

    private void OnError(object? sender, string e) =>
        _logger.Error("[{TAG}] StreamElements error: {ERROR}", "StreamElements", e);

    private void OnAuthenticated(SocketIOResponse response) =>
        _logger.Information("[{TAG}] Authenticated with StreamElements", "StreamElements");

    private void OnUnauthorized(SocketIOResponse response) =>
        _logger.Information("[{TAG}] Not authorized to use the StreamElements Realtime API", "StreamElements");

    private void OnEvent(SocketIOResponse response) {
        SocketEvent sockEvent = response.GetValue<SocketEvent>();

        if (sockEvent == null) {
            _logger.Error("[{TAG}] Invalid event data recieved", "StreamElements");
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
                _logger.Debug("[{TAG}] Ignoring unsupported event of type: {TYPE}", "StreamElements", sockEvent.Type);
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
