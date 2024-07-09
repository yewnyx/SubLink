using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Serilog;
using xyz.yewnyx.SubLink.Fansly.FanslyClient;

namespace xyz.yewnyx.SubLink.Fansly.Services;

[UsedImplicitly]
internal sealed partial class FanslyService {
    private readonly ILogger _logger;
    private readonly IHostApplicationLifetime _applicationLifetime;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
    private readonly IOptionsMonitor<FanslySettings> _settingsMonitor;
    private FanslySettings _settings;

    private readonly FanslyWSClient _fansly;

    private readonly FanslyRules _rules;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "Shhh")]
    private IServiceScope? _fanslyLoggedInScope;

    public FanslyService(
        ILogger logger,
        IHostApplicationLifetime applicationLifetime,
        IServiceScopeFactory serviceScopeFactory,
        IOptionsMonitor<FanslySettings> settingsMonitor,
        FanslyWSClient fanslyClient,
        FanslyRules rules) {
        _logger = logger;
        _applicationLifetime = applicationLifetime;
        _serviceScopeFactory = serviceScopeFactory;
        _settingsMonitor = settingsMonitor;
        _settingsMonitor.OnChange(UpdateFanslySettings);
        _settings = _settingsMonitor.CurrentValue;

        _fansly = fanslyClient ?? throw new ArgumentNullException(nameof(fanslyClient));

        _rules = rules;

        WireCallbacks();
    }

    private void UpdateFanslySettings(FanslySettings settings) => _settings = settings;

    public async Task StartAsync() {
        if (string.IsNullOrWhiteSpace(_settings.Username) ||
            string.IsNullOrWhiteSpace(_settings.Token)) {
            _logger.Warning("[{TAG}] Invalid config, skipping", Platform.PlatformName);
            return;
        }

        if (await _fansly.ConnectAsync(_settings.Token, _settings.Username)) {
            _logger.Information("[{TAG}] Connected to socket", Platform.PlatformName);
            _fanslyLoggedInScope = _serviceScopeFactory.CreateScope();
        } else {
            _logger.Warning("[{TAG}] Failed to connect to socket", Platform.PlatformName);
            _applicationLifetime.StopApplication();
        }
    }

    public async Task StopAsync() =>
        await _fansly.DisconnectAsync();
}
