using Microsoft.CodeAnalysis.CSharp.Scripting.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace xyz.yewnyx.SubLink;

public interface IService {
    public Task Start();
    public Task Stop();
}

public class SubLinkService<T1, T2> : BackgroundService where T1 : BaseCompilerService where T2 : IService {
    private readonly ILogger _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public SubLinkService(ILogger logger, IServiceScopeFactory serviceScopeFactory) {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
        using var sublinkScope = _serviceScopeFactory.CreateScope();

        var compiler = sublinkScope.ServiceProvider.GetService<T1>()!;
        var provider = new PhysicalFileProvider(Directory.GetCurrentDirectory());
        var script = provider.GetFileInfo("SubLink.cs");
        var scriptFunc = await compiler.CompileSource(script, stoppingToken);
        
        var service = sublinkScope.ServiceProvider.GetService<T2>()!;
        try {
            await service.Start();
            var returnValue = await scriptFunc();
            if (returnValue != null) {
                _logger.Debug("Return value: {ReturnValue}", CSharpObjectFormatter.Instance.FormatObject(returnValue));
            }
            
            while (!stoppingToken.IsCancellationRequested) {
                using var loopScope = _serviceScopeFactory.CreateScope();
                await Task.Delay(Timeout.InfiniteTimeSpan, stoppingToken);
            }
        } finally {
            await service.Stop();
        }
    }
}
