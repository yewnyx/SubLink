using Serilog;
using SocketIOClient;
using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace xyz.yewnyx.SubLink.Fansly;

internal sealed class FanslyClient {
    private const string _socketUri = "wss://chatws.fansly.com/";
    private const string _accountUri = "https://apiv3.fansly.com/api/v1/account?usernames={0}&ngsw-bypass=true";
    private const string _streamUri = "https://apiv3.fansly.com/api/v1/streaming/channel/{0}?ngsw-bypass=true";

    private ILogger _logger;
    private readonly SocketIOClient.SocketIO _socket;
    private string _token = string.Empty;
    private string _username = string.Empty;

    //public event EventHandler<TipEventArgs>? TipEvent;

    public FanslyClient(ILogger logger) {
        _logger = logger;

        _socket = new(_socketUri, new() {
            AutoUpgrade = true,
            ConnectionTimeout = TimeSpan.FromSeconds(30),
            EIO = SocketIO.Core.EngineIO.V3,
            Reconnection = true,
            ReconnectionAttempts = 3,
            ReconnectionDelay = 500,
            RandomizationFactor = 0.5,
            Transport = SocketIOClient.Transport.TransportProtocol.WebSocket
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
        _logger.Information("[{TAG}] Connected to Fansly", "Fansly");
        //await _socket.EmitAsync("authenticate", new SocketAuth(_token));
    }

    private void OnDisconnected(object? sender, string e) =>
        _logger.Information("[{TAG}] Disconnected from Fansly", "Fansly");

    private void OnError(object? sender, string e) =>
        _logger.Error("[{TAG}] Fansly error: {ERROR}", "Fansly", e);

    private void OnReconnectAttempt(object? sender, int e) =>
        _logger.Debug("[{TAG}] Socket reconnect attempt #{e}", "Fansly", e);

    private void OnReconnected(object? sender, int e) =>
        _logger.Information("[{TAG}] Socket reconnected after {e} attempts", "Fansly", e);

    private void OnReconnectError(object? sender, Exception e) =>
        _logger.Error("[{TAG}] Socket reconnect error:", "Fansly", e);

    private void OnReconnectFailed(object? sender, EventArgs e) =>
        _logger.Error("[{TAG}] Socket reconnect failed", "Fansly");

    private void OnAuthenticated(SocketIOResponse response) =>
        _logger.Information("[{TAG}] Authenticated with Fansly", "Fansly");

    private void OnUnauthorized(SocketIOResponse response) =>
        _logger.Information("[{TAG}] Not authorized to use the Fansly Realtime API", "Fansly");

    private void OnEvent(SocketIOResponse response) {
    }

    public async Task<bool> ConnectAsync(string token, string username) {
        _token = token;
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
                    request.Headers.Add("authorization", _token);
                    request.Headers.Add("origin", "https://fansly.com");
                    request.Headers.Add("referer", "https://fansly.com/");
                    request.Headers.Add("sec-ch-ua", "Not.A/Brand\";v=\"8\", \"Chromium\";v=\"114\", \"Google Chrome\";v=\"114\"");
                    request.Headers.Add("sec-ch-ua-mobile", "?0");
                    request.Headers.Add("sec-ch-ua-platform", "\"Windows\"");
                    request.Headers.Add("sec-fetch-dest", "empty");
                    request.Headers.Add("sec-fetch-mode", "cors");
                    request.Headers.Add("sec-fetch-site", "same-site");
                    request.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/114.0.0.0 Safari/537.36");
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
                    request.Headers.Add("authorization", _token);
                    request.Headers.Add("origin", "https://fansly.com");
                    request.Headers.Add("referer", "https://fansly.com/");
                    request.Headers.Add("sec-ch-ua", "Not.A/Brand\";v=\"8\", \"Chromium\";v=\"114\", \"Google Chrome\";v=\"114\"");
                    request.Headers.Add("sec-ch-ua-mobile", "?0");
                    request.Headers.Add("sec-ch-ua-platform", "\"Windows\"");
                    request.Headers.Add("sec-fetch-dest", "empty");
                    request.Headers.Add("sec-fetch-mode", "cors");
                    request.Headers.Add("sec-fetch-site", "same-site");
                    request.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/114.0.0.0 Safari/537.36");
                    var response = await client.SendAsync(request);
                    streamJsonStr = await response.Content.ReadAsStringAsync();
                    stream = JsonSerializer.Deserialize<StreamInfoResponse>(streamJsonStr) ?? new();
                }

                if (!(stream.Success && stream.Response != null && stream.Response.Length > 0)) {
                    _logger.Error("[{TAG}] Stream offline", "Fansly");
                    return false;
                }
            }
            

            //await _socket.ConnectAsync();
            return true;
        } catch (Exception) {
            return false;
        }
    }

    public async Task DisconnectAsync() { await Task.CompletedTask; } /* =>
        await _socket.DisconnectAsync();*/
}
