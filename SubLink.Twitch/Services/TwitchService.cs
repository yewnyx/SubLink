using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization.Metadata;
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

namespace xyz.yewnyx.SubLink.Twitch.Services;

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

    private readonly TwitchRules _rules;

    private string? ChannelName;
    private string? ChannelId;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "Shhh")]
    private IServiceScope? _twitchLoggedInScope;

    public TwitchService(
        ILogger logger,
        IHostApplicationLifetime applicationLifetime,
        IServiceScopeFactory serviceScopeFactory,
        IOptionsMonitor<TwitchSettings> settingsMonitor,
        EventSubWebsocketClient eventSub,
        TwitchRules rules) {
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

    public async Task StartAsync() {
        if (string.IsNullOrWhiteSpace(_settings.ClientId) ||
            string.IsNullOrWhiteSpace(_settings.ClientSecret)) {
            _logger.Warning("[{TAG}] Invalid config, skipping", Platform.PlatformName);
            return;
        }

        _api.Settings.AccessToken = _settings.AccessToken;
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
        
        await ValidateOrUpdateAccessTokenAsync();
        // TODO: eventually, embedding a webview is preferable - that or using a server after all

        var credentials = new ConnectionCredentials(ChannelName, _settings.AccessToken);
        _client.Initialize(credentials, ChannelName);
        _client.Connect();

        if (await _eventSub.ConnectAsync()) {
            _logger.Information("[{TAG}] Connected to EventSub", Platform.PlatformName);
        } else {
            _logger.Warning("[{TAG}] Failed to connect to EventSub", Platform.PlatformName);
        }
    }

    public async Task StopAsync() {
        await _eventSub.DisconnectAsync();
    }

    private async Task OnWebsocketConnected(object? sender, WebsocketConnectedArgs e) {
        if (!e.IsRequestedReconnect) {
            await SubscribeAsync("channel.channel_points_custom_reward_redemption.add");
            await SubscribeAsync("channel.channel_points_custom_reward_redemption.update");
            await SubscribeAsync("channel.update");
            await SubscribeAsync("channel.cheer");
            await SubscribeAsync("channel.follow");
            await SubscribeAsync("channel.hype_train.begin");
            await SubscribeAsync("channel.hype_train.end");
            await SubscribeAsync("channel.hype_train.progress");
            await SubscribeAsync("channel.poll.begin");
            await SubscribeAsync("channel.poll.end");
            await SubscribeAsync("channel.poll.progress");
            await SubscribeAsync("channel.prediction.begin");
            await SubscribeAsync("channel.prediction.end");
            await SubscribeAsync("channel.prediction.lock");
            await SubscribeAsync("channel.prediction.progress");
            await SubscribeAsync("channel.raid");
            await SubscribeAsync("channel.subscribe");
            await SubscribeAsync("channel.subscription.end");
            await SubscribeAsync("channel.subscription.gift");
            await SubscribeAsync("channel.subscription.message");
            await SubscribeAsync("stream.offline");
            await SubscribeAsync("stream.online");
        }
    }

    private async Task OnWebsocketDisconnected(object? sender, EventArgs e) {
        _logger.Information("[{TAG}] sender: {Sender} event: {@E}", Platform.PlatformName, sender, e);
        await Task.CompletedTask;
    }
    private async Task OnWebsocketReconnected(object? sender, EventArgs e) {
        _logger.Information("[{TAG}] sender: {Sender} event: {@E}", Platform.PlatformName, sender, e);
        await Task.CompletedTask;
    }

    private async Task<CreateEventSubSubscriptionResponse> SubscribeAsync(string subscriptionType) {
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

    private async Task<AuthCodeResponse> LaunchOAuthFlowAsync() {
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

    private async Task ValidateOrUpdateAccessTokenAsync() {
        var accessToken = _settings.AccessToken;
        var refreshToken = _settings.RefreshToken;
        ValidateAccessTokenResponse? validation;

        do {
            // No info of any kind: start full OAuth flow
            if (!accessToken.HasAnything() && !refreshToken.HasAnything()) {
                _logger.Warning("[{TAG}] Twitch access is not configured, requesting Twitch authorization", Platform.PlatformName);

                var authCodeResponse = await LaunchOAuthFlowAsync();
                accessToken = authCodeResponse.AccessToken;
                refreshToken = authCodeResponse.RefreshToken;
                validation = await _api.Auth.ValidateAccessTokenAsync(accessToken);
                if (validation != null) { break; }
            }

            // Has a refresh token only, inexplicably, but whatever, try and refresh it
            if (!accessToken.HasAnything() && refreshToken.HasAnything()) {
                _logger.Warning("[{TAG}] Twitch access token is not configured, but refresh token is available", Platform.PlatformName);
                _logger.Information("[{TAG}] Attempting Twitch access token refresh", Platform.PlatformName);
                
                if (await _api.Auth.RefreshAuthTokenAsync(_settings.RefreshToken, _settings.ClientSecret, _settings.ClientId) is
                    { } refreshResponse) {
                    _logger.Verbose("Twitch access token refresh succeeded");
                    accessToken = refreshResponse.AccessToken;
                    refreshToken = refreshResponse.RefreshToken;
                    validation = await _api.Auth.ValidateAccessTokenAsync(accessToken);

                    if (validation != null)
                        break;
                } else {
                    _logger.Information("[{TAG}] Twitch access token refresh failed", Platform.PlatformName);
                    refreshToken = null;
                }
            }
            
            // Validate auth token
            validation = await _api.Auth.ValidateAccessTokenAsync(accessToken);
            
            if (validation != null)
                break;

            _logger.Warning("[{TAG}] Your auth token may have expired. Trying to refresh it", Platform.PlatformName);

            if (await _api.Auth.RefreshAuthTokenAsync(_settings.RefreshToken, _settings.ClientSecret, _settings.ClientId) is { } refresh2) {
                _logger.Verbose("[{TAG}] Access token refresh succeeded", Platform.PlatformName);
                accessToken = refresh2.AccessToken;
                refreshToken = refresh2.RefreshToken;
                validation = await _api.Auth.ValidateAccessTokenAsync(accessToken);

                if (validation != null)
                    break;
            }
            
            _logger.Warning("[{TAG}] Your access tokens seem to be invalid. Trying a last-ditch full re-auth", Platform.PlatformName);
            var acr2 = await LaunchOAuthFlowAsync();
            accessToken = acr2.AccessToken;
            refreshToken = acr2.RefreshToken;
            validation = await _api.Auth.ValidateAccessTokenAsync(accessToken);

            if (validation != null) {
                _logger.Information("[{TAG}] Your new OAuth token is ready. Please relaunch SubLink!", Platform.PlatformName);
                Console.ReadLine();
                _applicationLifetime.StopApplication();
                return;
            } else {
                _logger.Warning("[{TAG}] Something went wrong. Please send logs to the SubLink developers!", Platform.PlatformName);
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
            Platform.PlatformName, ChannelName, ChannelId, string.Join(", ", validation.Scopes));

        var json = await File.ReadAllTextAsync(Platform.PlatformConfigFile);
        var j = JsonNode.Parse(json, documentOptions: new JsonDocumentOptions { CommentHandling = JsonCommentHandling.Skip });

#pragma warning disable CS8602 // Dereference of a possibly null reference.
        j[Platform.PlatformName]["AccessToken"] = accessToken;
        j[Platform.PlatformName]["RefreshToken"] = refreshToken;

        var scopes = new JsonArray();
        validation.Scopes.ForEach(scope => scopes.Add(scope));
        j[Platform.PlatformName]["Scopes"] = scopes;
        await File.WriteAllTextAsync(Platform.PlatformConfigFile, j.ToJsonString(new JsonSerializerOptions {
            WriteIndented = true,
            TypeInfoResolver = new DefaultJsonTypeInfoResolver()
        }));
        _api.Settings.AccessToken = accessToken;
        
        _twitchLoggedInScope = _serviceScopeFactory.CreateScope();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
    }
}

internal static class StringUtil {
    internal static bool HasAnything(this string str) => !string.IsNullOrWhiteSpace(str);
}
