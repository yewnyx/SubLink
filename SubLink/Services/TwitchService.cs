using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Context;
using TwitchLib.Api;
using TwitchLib.Api.Auth;
using TwitchLib.Api.Core.Enums;
using TwitchLib.Api.Helix.Models.EventSub;
using TwitchLib.Client;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Models;
using TwitchLib.EventSub.Websockets;
using TwitchLib.EventSub.Websockets.Core.EventArgs;

namespace xyz.yewnyx.SubLink;

[UsedImplicitly]
internal sealed partial class TwitchService {
    private readonly ILogger _logger;
    private readonly IHostApplicationLifetime _applicationLifetime;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
    private readonly IOptionsMonitor<TwitchSettings> _settingsMonitor;
    private TwitchSettings _settings;
    
    private readonly TwitchAPI _api;
    private readonly TwitchClient _client;
    private readonly EventSubWebsocketClient _eventSub;

    private readonly ITwitchRules _rules;
    
    private string? ChannelName;
    private string? ChannelId;

    private IServiceScope _twitchLoggedInScope;
    
    public TwitchService(
        ILogger logger,
        IHostApplicationLifetime applicationLifetime,
        IServiceScopeFactory serviceScopeFactory,
        IOptionsMonitor<TwitchSettings> settingsMonitor,
        IServiceProvider serviceProvider,
        EventSubWebsocketClient eventSub,
        ITwitchRules rules) {

        Globals.serviceProvider = serviceProvider;
        
        _logger = logger;
        _applicationLifetime = applicationLifetime;
        _serviceScopeFactory = serviceScopeFactory;
        _settingsMonitor = settingsMonitor;
        _settingsMonitor.OnChange(UpdateTwitchSettings);
        _settings = _settingsMonitor.CurrentValue;
        
        _api = new TwitchAPI();
        var clientOptions = new ClientOptions {
            MessagesAllowedInPeriod = 750,
            ThrottlingPeriod = TimeSpan.FromSeconds(30),
        };
        var customClient = new WebSocketClient(clientOptions);
        _client = new TwitchClient(customClient);
        _client.OnJoinedChannel += OnJoinedChannel;
        _client.OnMessageReceived += OnMessageReceived;

        _eventSub = eventSub ?? throw new ArgumentNullException(nameof(eventSub));
        
        _rules = rules;

        WireCallbacks();
    }

    private void UpdateTwitchSettings(TwitchSettings twitchSettings) => _settings = twitchSettings;

    public async Task Start() {
        var accessToken = _settings.AccessToken;

        _api.Settings.AccessToken = accessToken;
        _api.Settings.ClientId = _settings.ClientId;
        _api.Settings.Scopes = new List<AuthScopes> {
            AuthScopes.Helix_Bits_Read,
            AuthScopes.Chat_Read,
            AuthScopes.Chat_Edit,
            AuthScopes.Helix_Channel_Manage_Redemptions,
            AuthScopes.Helix_Channel_Read_Redemptions,
            AuthScopes.Helix_Channel_Read_Hype_Train,
            AuthScopes.Helix_Channel_Manage_Polls,
            AuthScopes.Helix_Channel_Read_Polls,
            AuthScopes.Helix_Channel_Read_VIPs,
        };
        
        await ValidateOrUpdateAccessToken();
        // TODO: eventually, embedding a webview is preferable - that or using a server after all

        var credentials = new ConnectionCredentials(ChannelName, accessToken);
        _client.Initialize(credentials, ChannelName);
        _client.Connect();

        if (await _eventSub.ConnectAsync()) {
            _logger.Information("[{TAG}] Connected to EventSub", "Twitch");
        } else {
            _logger.Warning("[{TAG}] Failed to connect to EventSub", "Twitch");
        }
    }

    public async Task Stop() {
        await _eventSub.DisconnectAsync();
    }

    private void OnWebsocketConnected(object? sender, WebsocketConnectedArgs e) {
        Task.Run(async () => {
            if (!e.IsRequestedReconnect) {
                await Task.Delay(500);
                await Task.WhenAll(
                    Subscribe("channel.channel_points_custom_reward_redemption.add"),
                    Subscribe("channel.channel_points_custom_reward_redemption.update"),
                    Subscribe("channel.update"),
                    Subscribe("channel.cheer"),
                    Subscribe("channel.follow"),
                    Subscribe("channel.hype_train.begin"),
                    Subscribe("channel.hype_train.end"),
                    Subscribe("channel.hype_train.progress"),
                    Subscribe("channel.poll.begin"),
                    Subscribe("channel.poll.end"),
                    Subscribe("channel.poll.progress"),
                    Subscribe("channel.prediction.begin"),
                    Subscribe("channel.prediction.end"),
                    Subscribe("channel.prediction.lock"),
                    Subscribe("channel.prediction.progress"),
                    Subscribe("channel.raid"),
                    Subscribe("channel.subscribe"),
                    Subscribe("channel.subscription.end"),
                    Subscribe("channel.subscription.gift"),
                    Subscribe("channel.subscription.message"),
                    Subscribe("stream.offline"),
                    Subscribe("stream.online")
                );
            }
        });
    }

    private void OnWebsocketDisconnected(object? sender, EventArgs e) { Task.Run(() => {
        _logger.Information("[{TAG}] sender: {Sender} event: {@E}", "Twitch", sender, e);
        return Task.CompletedTask;
    }); }
    private void OnWebsocketReconnected(object? sender, EventArgs e) { Task.Run(() => {
        _logger.Information("[{TAG}] sender: {Sender} event: {@E}", "Twitch", sender, e);
        return Task.CompletedTask;
    }); }

    private async Task<CreateEventSubSubscriptionResponse> Subscribe(string subscriptionType) {
        return await _api.Helix.EventSub.CreateEventSubSubscriptionAsync(
            subscriptionType,
            "1",
            new Dictionary<string, string>{{"broadcaster_user_id", ChannelId!}},
            EventSubTransportMethod.Websocket, 
            _eventSub.SessionId,
            null,
            null,
            _settings.ClientId,
            _settings.AccessToken);
    }

    private async Task<AuthCodeResponse> LaunchOAuthFlow() {
        const string redirectUri = "http://localhost:50666/authorize/";
        
        using var listener = new HttpListener();
        try {
            listener.Prefixes.Add(redirectUri);
        
            var state = System.Security.Cryptography.RandomNumberGenerator.GetInt32(0, 1000000).ToString("D6");
            var url = _api.Auth.GetAuthorizationCodeUrl(
                redirectUri,
                new [] {
                    AuthScopes.Chat_Read,
                    AuthScopes.Chat_Edit,
                    AuthScopes.Helix_Bits_Read,
                    AuthScopes.Helix_Channel_Read_Subscriptions,
                    AuthScopes.Helix_Channel_Manage_Redemptions,
                    AuthScopes.Helix_Channel_Read_Redemptions,
                    AuthScopes.Helix_Channel_Read_Hype_Train,
                    AuthScopes.Helix_Channel_Manage_Polls,
                    AuthScopes.Helix_Channel_Read_Polls,
                    AuthScopes.Helix_Channel_Read_VIPs,
                },
                state: state,
                clientId: _settings.ClientId);
            
            listener.Start();
            
            var processInfo = new ProcessStartInfo {FileName = url, UseShellExecute = true};
            Process.Start(processInfo);
            
            var context = await listener.GetContextAsync().ConfigureAwait(false);
            var queryString = context.Request.QueryString;
            await using (var writer = new StreamWriter(context.Response.OutputStream)) {
                await writer.WriteLineAsync("Success!");
                await writer.FlushAsync();
            }
            context.Response.OutputStream.Close();

            var code = queryString["code"];
            
            // TODO: verify the state code
            
            var authCodeResponse = await _api.Auth.GetAccessTokenFromCodeAsync(code, _settings.ClientSecret, redirectUri, _settings.ClientId);
            return authCodeResponse;
        } finally {
            listener.Close();
        }
    }

    private async Task ValidateOrUpdateAccessToken() {
        var accessToken = _settings.AccessToken;
        var refreshToken = _settings.RefreshToken;
        ValidateAccessTokenResponse? validation;
        do {
            // No info of any kind: start full OAuth flow
            if (!accessToken.HasAnything() && !refreshToken.HasAnything()) {
                _logger.Warning("[{TAG}] Twitch access is not configured, requesting Twitch authorization", "Twitch");

                var authCodeResponse = await LaunchOAuthFlow();
                accessToken = authCodeResponse.AccessToken;
                refreshToken = authCodeResponse.RefreshToken;
                validation = await _api.Auth.ValidateAccessTokenAsync(accessToken);
                if (validation != null) { break; }
            }

            // Has a refresh token only, inexplicably, but whatever, try and refresh it
            if (!accessToken.HasAnything() && refreshToken.HasAnything()) {
                _logger.Warning("[{TAG}] Twitch access token is not configured, but refresh token is available", "Twitch");
                _logger.Information("[{TAG}] Attempting Twitch access token refresh", "Twitch");
                if (await _api.Auth.RefreshAuthTokenAsync(_settings.RefreshToken, _settings.ClientSecret, _settings.ClientId) is
                    { } refreshResponse) {
                    _logger.Verbose("Twitch access token refresh succeeded");
                    accessToken = refreshResponse.AccessToken;
                    refreshToken = refreshResponse.RefreshToken;
                    validation = await _api.Auth.ValidateAccessTokenAsync(accessToken);
                    if (validation != null) { break; }
                } else {
                    _logger.Information("[{TAG}] Twitch access token refresh failed", "Twitch");
                    refreshToken = null;
                }
            }
            
            // Validate auth token
            validation = await _api.Auth.ValidateAccessTokenAsync(accessToken);
            if (validation != null) { break; }
            _logger.Warning("[{TAG}] Your auth token may have expired. Trying to refresh it", "Twitch");

            if (await _api.Auth.RefreshAuthTokenAsync(_settings.RefreshToken, _settings.ClientSecret, _settings.ClientId) is { } refresh2) {
                _logger.Verbose("[{TAG}] Access token refresh succeeded", "Twitch");
                accessToken = refresh2.AccessToken;
                refreshToken = refresh2.RefreshToken;
                validation = await _api.Auth.ValidateAccessTokenAsync(accessToken);
                if (validation != null) { break; }
            }
            
            _logger.Warning("[{TAG}] Your access tokens seem to be invalid. Trying a last-ditch full re-auth", "Twitch");
            var acr2 = await LaunchOAuthFlow();
            accessToken = acr2.AccessToken;
            refreshToken = acr2.RefreshToken;
            validation = await _api.Auth.ValidateAccessTokenAsync(accessToken);
            if (validation != null) {
                _logger.Information("[{TAG}] Your new OAuth token is ready. Please relaunch SubLink!", "Twitch");
                Console.ReadLine();
                _applicationLifetime.StopApplication();
                return;
            } else {
                _logger.Warning("[{TAG}] Something went wrong. Please send logs to the SubLink developers!", "Twitch");
                Console.ReadLine();
                _applicationLifetime.StopApplication();
                return;
            }
        } while (false);

        ChannelName = validation.Login;
        ChannelId = validation.UserId;

        using (await GlobalLogContext.LockAsync()) {
            GlobalLogContext.PushProperty("ChannelName", ChannelName);
        }
        
        _logger.Information("[{TAG}] Validated token for {ChannelName} ({ChannelId}) with scopes: {Scopes}",
            "Twitch", ChannelName, ChannelId, string.Join(", ", validation.Scopes));

        var json = await File.ReadAllTextAsync("settings.json");
        var j = JsonNode.Parse(json,
            documentOptions: new JsonDocumentOptions {CommentHandling = JsonCommentHandling.Skip});
        j["Twitch"]["AccessToken"] = accessToken;
        j["Twitch"]["RefreshToken"] = refreshToken;

        var scopes = new JsonArray();
        validation.Scopes.ForEach(scope => scopes.Add(scope));
        j["Twitch"]["Scopes"] = scopes;
        await File.WriteAllTextAsync("settings.json", j.ToJsonString(new JsonSerializerOptions{ WriteIndented = true }));
        _api.Settings.AccessToken = accessToken;
        
        _twitchLoggedInScope = _serviceScopeFactory.CreateScope();
    }
}

internal static class StringUtil {
    internal static bool HasAnything(this string str) => !string.IsNullOrWhiteSpace(str);
}