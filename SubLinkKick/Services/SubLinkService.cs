using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Scripting.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Serilog;
using xyz.yewnyx;
using xyz.yewnyx.SubLink;

internal class SubLinkService : BackgroundService {
    private readonly ILogger _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public SubLinkService(ILogger logger, IServiceScopeFactory serviceScopeFactory) {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
        using var sublinkScope = _serviceScopeFactory.CreateScope();

        var compiler = sublinkScope.ServiceProvider.GetService<CompilerService>()!;
        var provider = new PhysicalFileProvider(Directory.GetCurrentDirectory());
        var script = provider.GetFileInfo("SubLink.cs");
        var scriptFunc = await compiler.CompileSource(script, stoppingToken);
        
        var twitchService = sublinkScope.ServiceProvider.GetService<TwitchService>()!;
        try {
            await twitchService.Start();
            var returnValue = await scriptFunc();
            if (returnValue != null) {
                _logger.Debug("Return value: {ReturnValue}", CSharpObjectFormatter.Instance.FormatObject(returnValue));
            }
            
            while (!stoppingToken.IsCancellationRequested) {
                using var loopScope = _serviceScopeFactory.CreateScope();
                await Task.Delay(Timeout.InfiniteTimeSpan, stoppingToken);
            }
        } finally {
            await twitchService.Stop();
        }
    }
}