using BuildSoft.VRChat.Osc;
using Microsoft.CodeAnalysis.CSharp.Scripting.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Serilog;
using System.Net;

namespace xyz.yewnyx.SubLink.Services;

internal class SubLinkService(ILogger logger,
    IServiceScopeFactory serviceScopeFactory,
    IOptions<SubLinkSettings> settings
) : BackgroundService {
    private readonly ILogger _logger = logger;
    private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;
    private readonly SubLinkSettings _settings = settings.Value;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
        using var sublinkScope = _serviceScopeFactory.CreateScope();
        var scopedSvcProvider = sublinkScope.ServiceProvider;

        // We only change the IP address when it's valid
        if (IPAddress.TryParse(_settings.OscIPAddress, out var ip))
            OscConnectionSettings.VrcIPAddress = _settings.OscIPAddress;

        // We MUST clamp the port to prevent user error, only change it when it's > 0
        int oscPort = int.Clamp(_settings.OscPort, ushort.MinValue, ushort.MaxValue);
        if (oscPort > 0)
            OscConnectionSettings.SendPort = oscPort;

        // We have to make sure the script name is valid and exists, if not, use the default
        string scriptName = "SubLink.cs";
        if ((!string.IsNullOrWhiteSpace(_settings.ScriptName)) && File.Exists(_settings.ScriptName))
            scriptName = _settings.ScriptName;

        var compiler = scopedSvcProvider.GetService<CompilerService>()!;
        var provider = new PhysicalFileProvider(Directory.GetCurrentDirectory());
        var script = provider.GetFileInfo(scriptName);
        var scriptFunc = await compiler.CompileSource(script, scopedSvcProvider, stoppingToken);
        
        var oscSupportService = scopedSvcProvider.GetService<OSCSupportService>()!;

        try {
            oscSupportService.Start();

            foreach (var platform in HostGlobals.Platforms.Values) {
                await platform.Entry.StartServiceAsync();
            }

            var returnValue = await scriptFunc();

            if (returnValue != null)
                _logger.Debug("Return value: {ReturnValue}", CSharpObjectFormatter.Instance.FormatObject(returnValue));
            
            while (!stoppingToken.IsCancellationRequested) {
                using var loopScope = _serviceScopeFactory.CreateScope();
                await Task.Delay(Timeout.InfiniteTimeSpan, stoppingToken);
            }
        } finally {
            foreach (var platform in HostGlobals.Platforms.Values) {
                await platform.Entry.StopServiceAsync();
            }
        }
    }
}
