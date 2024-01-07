using BuildSoft.VRChat.Osc;
using Microsoft.CodeAnalysis.CSharp.Scripting.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Serilog;

namespace xyz.yewnyx.SubLink;

public interface IService {
    public Task Start();
    public Task Stop();
}

public class SubLinkService<TGlobals, TCompilerService, TService> : BackgroundService 
    where TGlobals : IGlobals 
    where TCompilerService : BaseCompilerService<TGlobals> 
    where TService : IService {
    private readonly ILogger _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
    private readonly IOptionsMonitor<SubLinkSettings> _settingsMonitor;
    private SubLinkSettings _settings;

    public SubLinkService(
        ILogger logger,
        IServiceScopeFactory serviceScopeFactory,
        IOptionsMonitor<SubLinkSettings> settingsMonitor
    ) {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;

        _settingsMonitor = settingsMonitor;
        _settingsMonitor.OnChange(UpdateSubLinkSettings);
        _settings = _settingsMonitor.CurrentValue;
    }

    private void UpdateSubLinkSettings(SubLinkSettings settings) => _settings = settings;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
        using var sublinkScope = _serviceScopeFactory.CreateScope();

        OscConnectionSettings.VrcIPAddress = string.IsNullOrWhiteSpace(_settings.OscIPAddress)
            ? "127.0.0.1"
            : _settings.OscIPAddress;
        OscConnectionSettings.SendPort = _settings.OscPort <= 0
            ? 9000
            : _settings.OscPort;
        string scriptName = string.IsNullOrWhiteSpace(_settings.ScriptName) || !File.Exists(_settings.ScriptName)
            ? "SubLink.cs"
            : _settings.ScriptName;

        var compiler = sublinkScope.ServiceProvider.GetService<TCompilerService>()!;
        var provider = new PhysicalFileProvider(Directory.GetCurrentDirectory());
        var script = provider.GetFileInfo(scriptName);
        var scriptFunc = await compiler.CompileSource(script, stoppingToken);
        
        var oscSupportService = sublinkScope.ServiceProvider.GetService<OSCSupportService<TGlobals>>()!;
        var service = sublinkScope.ServiceProvider.GetService<TService>()!;

        try {
            oscSupportService.Start();
            await service.Start();
            var returnValue = await scriptFunc();

            if (returnValue != null)
                _logger.Debug("Return value: {ReturnValue}", CSharpObjectFormatter.Instance.FormatObject(returnValue));
            
            while (!stoppingToken.IsCancellationRequested) {
                using var loopScope = _serviceScopeFactory.CreateScope();
                await Task.Delay(Timeout.InfiniteTimeSpan, stoppingToken);
            }
        } finally {
            await service.Stop();
        }
    }
}
