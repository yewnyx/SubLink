using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Serilog;
using System;
using System.Threading.Tasks;
using xyz.yewnyx.SubLink.Discord.Client;

namespace xyz.yewnyx.SubLink.Discord.Services;

[UsedImplicitly]
internal sealed partial class DiscordService {
    private readonly ILogger _logger;
    private readonly IHostApplicationLifetime _applicationLifetime;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
    private readonly IOptionsMonitor<DiscordSettings> _settingsMonitor;
    private DiscordSettings _settings;
    private DiscordClient _discord;

    private readonly DiscordRules _rules;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "Shhh")]
    private IServiceScope? _discordLoggedInScope;

    public DiscordService(
        ILogger logger,
        IHostApplicationLifetime applicationLifetime,
        IServiceScopeFactory serviceScopeFactory,
        IOptionsMonitor<DiscordSettings> settingsMonitor,
        DiscordRules rules) {
        _logger = logger;
        _applicationLifetime = applicationLifetime;
        _serviceScopeFactory = serviceScopeFactory;
        _settingsMonitor = settingsMonitor;
        _settingsMonitor.OnChange(UpdateDiscordSettings);
        _settings = _settingsMonitor.CurrentValue;

        _rules = rules;

        _discord = new(_logger);

        WireCallbacks();
    }

    private void UpdateDiscordSettings(DiscordSettings settings) => _settings = settings;

    public async Task StartAsync() {
        if (!_settings.Enabled) {
            _logger.Warning("[{TAG}] Disabled in config, skipping", Platform.PlatformName);
            return;
        }

        if (string.IsNullOrWhiteSpace(_settings.ClientID) ||
            string.IsNullOrWhiteSpace(_settings.ClientSecret)) {
            _logger.Warning("[{TAG}] Invalid config, skipping", Platform.PlatformName);
            return;
        }

        try
        {
            DiscordAuth auth = new(_settings.ClientID, _settings.ClientSecret);
            string accessToken = await auth.FetchAccessTokenAsync();
            _logger.Debug("[{TAG}] Access token retrieved successfully.", Platform.PlatformName);

            // Attempt to connect to any available Discord IPC pipe
            bool connected = false;
            _logger.Information("[{TAG}] Attempting to connect to Discord...", Platform.PlatformName);

            for (int i = 0; i < 10; i++) // Attempt discord-ipc-0 through discord-ipc-9
            {
                string pipeName = $"discord-ipc-{i}";
                _logger.Debug("[{TAG}] Attempting to connect to {pipeName}...", Platform.PlatformName, pipeName);

                var connectTask = Task.Run(() => _discord.Connect(pipeName)); // Start the connect task
                var timeoutTask = Task.Delay(TimeSpan.FromSeconds(2)); // 2-second timeout

                var completedTask = await Task.WhenAny(connectTask, timeoutTask);

                if (completedTask == connectTask) // Connection completed successfully
                {
                    try
                    {
                        await connectTask; // Ensure no exceptions occurred
                        connected = true;
                        _logger.Information("[{TAG}] Successfully connected to {pipeName}", Platform.PlatformName, pipeName);
                        break;
                    }
                    catch (Exception ex)
                    {
                        _logger.Debug("[{TAG}] Error connecting to {pipeName}: {ERROR}", Platform.PlatformName, pipeName, ex.Message);
                    }
                }
                else // Timeout
                {
                    _logger.Debug("[{TAG}] Connection attempt to {pipeName} timed out.", Platform.PlatformName, pipeName);
                }
            }

            if (!connected)
            {
                _logger.Information("[{TAG}] Failed to connect to Discord IPC pipes. Make sure Discord is running.", Platform.PlatformName);
                return;
            }

            // Timeout for the handshake process
            var handshakeTask = Task.Run(() => _discord.Handshake(_settings.ClientID));
            var handshakeTimeout = Task.Delay(TimeSpan.FromSeconds(3)); // 3-second timeout for handshake

            var completedHandshake = await Task.WhenAny(handshakeTask, handshakeTimeout);

            if (completedHandshake == handshakeTask)
            {
                var handshakeResponse = handshakeTask.Result;
                _logger.Debug("[{TAG}] Handshake completed successfully.", Platform.PlatformName);
            }
            else
            {
                _logger.Warning("[{TAG}] Handshake timed out. Try restarting discord.", Platform.PlatformName);
                return;
            }

            // Timeout for the authentication process
            var authPayload = DiscordIpcMessage.Authenticate(accessToken);
            var authTask = Task.Run(() => _discord.SendDataAndWait(1, authPayload));
            var authTimeout = Task.Delay(TimeSpan.FromSeconds(3)); // 3-second timeout for authentication

            var completedAuth = await Task.WhenAny(authTask, authTimeout);

            if (completedAuth == authTask)
            {
                var authResponse = authTask.Result;
                _logger.Information("[{TAG}] Authenticated successfully!", Platform.PlatformName);
                // Subscribe to common events
                _logger.Debug("[{TAG}] Subscribing to READY", Platform.PlatformName);
                _discord.SendDataAndWait(1, DiscordIpcMessage.Subscribe("READY"));
                _logger.Debug("[{TAG}] Subscribing to ERROR", Platform.PlatformName);
                _discord.SendDataAndWait(1, DiscordIpcMessage.Subscribe("ERROR"));
                _logger.Debug("[{TAG}] Subscribing to VOICE_CHANNEL_SELECT", Platform.PlatformName);
                _discord.SendDataAndWait(1, DiscordIpcMessage.Subscribe("VOICE_CHANNEL_SELECT"));
                _logger.Debug("[{TAG}] Subscribing to VOICE_SETTINGS_UPDATE", Platform.PlatformName);
                _discord.SendDataAndWait(1, DiscordIpcMessage.Subscribe("VOICE_SETTINGS_UPDATE"));
                _logger.Debug("[{TAG}] Subscribing to VOICE_CONNECTION_STATUS", Platform.PlatformName);
                _discord.SendDataAndWait(1, DiscordIpcMessage.Subscribe("VOICE_CONNECTION_STATUS"));

                if (!string.IsNullOrEmpty(_settings.DefaultGuildId))
                {
                    _logger.Debug("[{TAG}] Subscribing to GUILD_STATUS for {defaultGuildId}", Platform.PlatformName, _settings.DefaultGuildId);
                    _discord.SendDataAndWait(1, DiscordIpcMessage.Subscribe("GUILD_STATUS", new { guild_id = _settings.DefaultGuildId }));
                }
                else
                {
                    _logger.Debug("[{TAG}] DefaultGuildId empty; skipping GUILD_STATUS subscription", Platform.PlatformName);
                }

                _discord.SendDataAndWait(1, DiscordIpcMessage.Subscribe("GUILD_CREATE"));
                _discord.SendDataAndWait(1, DiscordIpcMessage.Subscribe("CHANNEL_CREATE"));

                if (!string.IsNullOrEmpty(_settings.DefaultChannelId))
                {
                    _logger.Debug("[{TAG}] Subscribing to voice and message events for channel {defaultChannelId}", Platform.PlatformName, _settings.DefaultChannelId);
                    var chArgs = new { channel_id = _settings.DefaultChannelId };
                    _discord.SendDataAndWait(1, DiscordIpcMessage.Subscribe("VOICE_STATE_CREATE", chArgs));
                    _discord.SendDataAndWait(1, DiscordIpcMessage.Subscribe("VOICE_STATE_UPDATE", chArgs));
                    _discord.SendDataAndWait(1, DiscordIpcMessage.Subscribe("VOICE_STATE_DELETE", chArgs));
                    _discord.SendDataAndWait(1, DiscordIpcMessage.Subscribe("SPEAKING_START", chArgs));
                    _discord.SendDataAndWait(1, DiscordIpcMessage.Subscribe("SPEAKING_STOP", chArgs));
                    _discord.SendDataAndWait(1, DiscordIpcMessage.Subscribe("MESSAGE_CREATE", chArgs));
                    _discord.SendDataAndWait(1, DiscordIpcMessage.Subscribe("MESSAGE_UPDATE", chArgs));
                    _discord.SendDataAndWait(1, DiscordIpcMessage.Subscribe("MESSAGE_DELETE", chArgs));
                }
                else
                {
                    _logger.Debug("[{TAG}] DefaultChannelId empty; skipping channel-specific subscriptions", Platform.PlatformName);
                }

                _discord.SendDataAndWait(1, DiscordIpcMessage.Subscribe("NOTIFICATION_CREATE"));
                _discord.SendDataAndWait(1, DiscordIpcMessage.Subscribe("ACTIVITY_JOIN"));
                _discord.SendDataAndWait(1, DiscordIpcMessage.Subscribe("ACTIVITY_SPECTATE"));
                _discord.SendDataAndWait(1, DiscordIpcMessage.Subscribe("ACTIVITY_JOIN_REQUEST"));

                _discord.StartListening();
            }
            else
            {
                _logger.Error("[{TAG}] Authentication timed out. Try restarting discord.", Platform.PlatformName);
                return;
            }

            _logger.Debug("[{TAG}] Discord Platform started!", Platform.PlatformName);
            _discordLoggedInScope = _serviceScopeFactory.CreateScope();
            _rules.SetService(this);
            return;
        }
        catch (Exception ex)
        {
            _logger.Error("[{TAG}] Error during start: {ERROR}", Platform.PlatformName, ex.Message);
            return;
        }
    }

    public async Task StopAsync() =>
        await Task.CompletedTask;
}
