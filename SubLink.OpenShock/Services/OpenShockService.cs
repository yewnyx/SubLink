using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using OpenShock.SDK.CSharp;
using Serilog;
using System.Threading.Tasks;

namespace xyz.yewnyx.SubLink.OpenShock.Services;

[UsedImplicitly]
internal sealed partial class OpenShockService {
    private readonly ILogger _logger;
    private readonly IHostApplicationLifetime _applicationLifetime;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
    private readonly IOptionsMonitor<OpenShockSettings> _settingsMonitor;
    private OpenShockSettings _settings;

    private readonly ApiClientOptions.ProgramInfo _progInfo;
    private OpenShockApiClient? _openShock;

    private readonly OpenShockRules _rules;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "Shhh")]
    private IServiceScope? _openShockLoggedInScope;

    public OpenShockService(
        ILogger logger,
        IHostApplicationLifetime applicationLifetime,
        IServiceScopeFactory serviceScopeFactory,
        IOptionsMonitor<OpenShockSettings> settingsMonitor,
        OpenShockRules rules) {
        _logger = logger;
        _applicationLifetime = applicationLifetime;
        _serviceScopeFactory = serviceScopeFactory;
        _settingsMonitor = settingsMonitor;
        _settingsMonitor.OnChange(UpdateOpenShockSettings);
        _settings = _settingsMonitor.CurrentValue;

        _progInfo = new() {
            Name = $"SubLink.{Platform.PlatformName}",
            Version = typeof(Platform).Assembly.GetName().Version ?? new("1.0.0")
        };

        _rules = rules;
    }

    private void UpdateOpenShockSettings(OpenShockSettings settings) => _settings = settings;

    public async Task StartAsync() {
        if (!_settings.Enabled) {
            _logger.Warning("[{TAG}] Disabled in config, skipping", Platform.PlatformName);
            return;
        }

        if (string.IsNullOrWhiteSpace(_settings.Server) ||
            string.IsNullOrWhiteSpace(_settings.Token)) {
            _logger.Warning("[{TAG}] Invalid config, skipping", Platform.PlatformName);
            return;
        }

        _openShock = new(new ApiClientOptions {
            Server = new(_settings.Server),
            Token = _settings.Token,
            Program = _progInfo
        });

        _logger.Information("[{TAG}] Connected to API", Platform.PlatformName);
        _openShockLoggedInScope = _serviceScopeFactory.CreateScope();
        _rules.SetService(this);
        await Task.CompletedTask;
    }

    public async Task StopAsync() =>
        await Task.CompletedTask;
}
