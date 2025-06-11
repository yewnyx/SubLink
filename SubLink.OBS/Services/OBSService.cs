using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Serilog;
using xyz.yewnyx.SubLink.OBS.OBSClient;

namespace xyz.yewnyx.SubLink.OBS.Services;

[UsedImplicitly]
internal sealed partial class OBSService {
    private readonly ILogger _logger;
    private readonly IHostApplicationLifetime _applicationLifetime;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
    private readonly IOptionsMonitor<OBSSettings> _settingsMonitor;
    private OBSSettings _settings;

    private readonly OBSSocketClient _obs;

    private readonly OBSRules _rules;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "Shhh")]
    private IServiceScope? _obsLoggedInScope;

    public OBSService(
        ILogger logger,
        IHostApplicationLifetime applicationLifetime,
        IServiceScopeFactory serviceScopeFactory,
        IOptionsMonitor<OBSSettings> settingsMonitor,
        OBSSocketClient obsClient,
        OBSRules rules) {
        _logger = logger;
        _applicationLifetime = applicationLifetime;
        _serviceScopeFactory = serviceScopeFactory;
        _settingsMonitor = settingsMonitor;
        _settingsMonitor.OnChange(UpdateOBSSettings);
        _settings = _settingsMonitor.CurrentValue;

        _obs = obsClient ?? throw new ArgumentNullException(nameof(obsClient));

        _rules = rules;

        WireCallbacks();
    }

    private void UpdateOBSSettings(OBSSettings obsSettings) => _settings = obsSettings;

    public async Task StartAsync() {
        if (!_settings.Enabled) {
            _logger.Warning("[{TAG}] Disabled in config, skipping", Platform.PlatformName);
            return;
        }

        if (string.IsNullOrWhiteSpace(_settings.ServerIp) ||
            _settings.ServerPort < ushort.MinValue ||
            _settings.ServerPort > ushort.MaxValue ||
            string.IsNullOrWhiteSpace(_settings.ServerPassword)) {
            _logger.Warning("[{TAG}] Invalid config, skipping", Platform.PlatformName);
            return;
        }

        if (await _obs.ConnectAsync(_settings.ServerIp, _settings.ServerPort, _settings.ServerPassword)) {
            _obsLoggedInScope = _serviceScopeFactory.CreateScope();
        } else {
            _logger.Warning("[{TAG}] Failed to connect to websocket", Platform.PlatformName);
            _applicationLifetime.StopApplication();
        }
    }

    public async Task StopAsync() =>
        await _obs.DisconnectAsync();
}
