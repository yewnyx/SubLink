using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Serilog;
using xyz.yewnyx.SubLink.Fansly;

namespace xyz.yewnyx.SubLink;

[UsedImplicitly]
internal sealed partial class FanslyService : IService {
    private readonly ILogger _logger;
    private readonly IHostApplicationLifetime _applicationLifetime;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
    private readonly IOptionsMonitor<FanslySettings> _settingsMonitor;
    private FanslySettings _settings;

    private readonly FanslyClient _fansly;

    private readonly IFanslyRules _rules;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "Shhh")]
    private IServiceScope? _fanslyLoggedInScope;

    public FanslyService(
        ILogger logger,
        FanslyGlobals globals,
        IHostApplicationLifetime applicationLifetime,
        IServiceScopeFactory serviceScopeFactory,
        IOptionsMonitor<FanslySettings> settingsMonitor,
        IServiceProvider serviceProvider,
        FanslyClient fanslyClient,
        IFanslyRules rules)
    {
        _logger = logger;
        globals.serviceProvider = serviceProvider;
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

    public async Task Start() {
        if (await _fansly.ConnectAsync(_settings.Token, _settings.Username)) {
            _logger.Information("[{TAG}] Connected to socket", "Fansly");
            _fanslyLoggedInScope = _serviceScopeFactory.CreateScope();
        } else {
            _logger.Warning("[{TAG}] Failed to connect to socket", "Fansly");
            _applicationLifetime.StopApplication();
        }
    }

    public async Task Stop() =>
        await _fansly.DisconnectAsync();
}
