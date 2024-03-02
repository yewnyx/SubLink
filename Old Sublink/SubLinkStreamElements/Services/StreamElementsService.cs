using System;
using System.Net.WebSockets;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Serilog;
using xyz.yewnyx.SubLink.StreamElements;

namespace xyz.yewnyx.SubLink;

[UsedImplicitly]
internal sealed partial class StreamElementsService : IService {
    private readonly ILogger _logger;
    private readonly IHostApplicationLifetime _applicationLifetime;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
    private readonly IOptionsMonitor<StreamElementsSettings> _settingsMonitor;
    private StreamElementsSettings _settings;

    private readonly StreamElementsClient _streamElements;

    private readonly IStreamElementsRules _rules;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "Shhh")]
    private IServiceScope? _streamElementsLoggedInScope;

    public StreamElementsService(
        ILogger logger,
        StreamElementsGlobals globals,
        IHostApplicationLifetime applicationLifetime,
        IServiceScopeFactory serviceScopeFactory,
        IOptionsMonitor<StreamElementsSettings> settingsMonitor,
        IServiceProvider serviceProvider,
        StreamElementsClient streamElementsClient,
        IStreamElementsRules rules)
    {
        _logger = logger;
        globals.serviceProvider = serviceProvider;
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

    public async Task Start() {
        if (await _streamElements.ConnectAsync(_settings.JWTToken)) {
            _streamElementsLoggedInScope = _serviceScopeFactory.CreateScope();
        } else {
            _logger.Warning("[{TAG}] Failed to connect to StreamElements", "StreamElements");
            _applicationLifetime.StopApplication();
        }
    }

    public async Task Stop() =>
        await _streamElements.DisconnectAsync();
}
