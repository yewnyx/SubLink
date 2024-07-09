using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Serilog;
using xyz.yewnyx.SubLink.StreamElements.SEClient;

namespace xyz.yewnyx.SubLink.StreamElements.Services;

[UsedImplicitly]
internal sealed partial class StreamElementsService {
    private readonly ILogger _logger;
    private readonly IHostApplicationLifetime _applicationLifetime;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
    private readonly IOptionsMonitor<StreamElementsSettings> _settingsMonitor;
    private StreamElementsSettings _settings;

    private readonly StreamElementsClient _streamElements;

    private readonly StreamElementsRules _rules;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "Shhh")]
    private IServiceScope? _streamElementsLoggedInScope;

    public StreamElementsService(
        ILogger logger,
        IHostApplicationLifetime applicationLifetime,
        IServiceScopeFactory serviceScopeFactory,
        IOptionsMonitor<StreamElementsSettings> settingsMonitor,
        StreamElementsClient streamElementsClient,
        StreamElementsRules rules) {
        _logger = logger;
        _applicationLifetime = applicationLifetime;
        _serviceScopeFactory = serviceScopeFactory;
        _settingsMonitor = settingsMonitor;
        _settingsMonitor.OnChange(UpdateStreamElementsSettings);
        _settings = _settingsMonitor.CurrentValue;

        _streamElements = streamElementsClient ?? throw new ArgumentNullException(nameof(streamElementsClient));

        _rules = rules;

        WireCallbacks();
    }

    private void UpdateStreamElementsSettings(StreamElementsSettings settings) => _settings = settings;

    public async Task StartAsync() {
        if (string.IsNullOrWhiteSpace(_settings.JWTToken)) {
            _logger.Warning("[{TAG}] Invalid config, skipping", Platform.PlatformName);
            return;
        }

        if (await _streamElements.ConnectAsync(_settings.JWTToken)) {
            _streamElementsLoggedInScope = _serviceScopeFactory.CreateScope();
        } else {
            _logger.Warning("[{TAG}] Failed to connect to socket", Platform.PlatformName);
            _applicationLifetime.StopApplication();
        }
    }

    public async Task StopAsync() =>
        await _streamElements.DisconnectAsync();
}
