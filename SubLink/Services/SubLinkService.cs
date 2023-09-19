using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Scripting.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
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

        var ts = sublinkScope.ServiceProvider.GetService<IOptions<TwitchSettings>>();
        var sps = sublinkScope.ServiceProvider.GetService<IOptions<StreamPadSettings>>();
        bool twitchConfigured = !string.IsNullOrWhiteSpace(ts!.Value.ClientId) && !string.IsNullOrWhiteSpace(ts.Value.ClientSecret);
        bool streamPadConfigured = !string.IsNullOrWhiteSpace(sps!.Value.WebSocketUrl) && !string.IsNullOrWhiteSpace(sps.Value.ChannelId);
        if (!twitchConfigured && !streamPadConfigured)
        {
            _logger.Error("You need to set up TwitchService and/or StreamPadService, check settings.json.");
            return;
        }

        var streamPadService = sublinkScope.ServiceProvider.GetService<StreamPadService>()!;
        var twitchService = sublinkScope.ServiceProvider.GetService<TwitchService>()!;
        
        try {
            if (twitchConfigured)
            {
                await twitchService.Start();
            }
            if (streamPadConfigured)
            {
                await streamPadService.Start();
            }
            var returnValue = await scriptFunc();
            if (returnValue != null) {
                _logger.Debug("Return value: {ReturnValue}", CSharpObjectFormatter.Instance.FormatObject(returnValue));
            }
            
            while (!stoppingToken.IsCancellationRequested) {
                using var loopScope = _serviceScopeFactory.CreateScope();
                await Task.Delay(Timeout.InfiniteTimeSpan, stoppingToken);
            }
        } finally {
            if (twitchConfigured)
            {
                await twitchService.Stop();
            }
            if (streamPadConfigured)
            {
                await streamPadService.Stop();
            }
        }
    }
}