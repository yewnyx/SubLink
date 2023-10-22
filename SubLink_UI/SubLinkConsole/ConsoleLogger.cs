using System.Text;
using FlowGraph.Logger;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace tech.sublink.SubLinkConsole;

class ConsoleLogger : ILog {
    private readonly ILogger _logger;
    private readonly IHostApplicationLifetime _applicationLifetime;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public ConsoleLogger(
        ILogger logger,
        IHostApplicationLifetime applicationLifetime,
        IServiceScopeFactory serviceScopeFactory
    ) {
        _logger = logger;
        _applicationLifetime = applicationLifetime;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public void Close() { }

    public void Write(LogVerbosity verbose, string msg) {
        switch (verbose) {
            case LogVerbosity.Trace:   _logger.Verbose(msg);     break;
            case LogVerbosity.Debug:   _logger.Debug(msg);       break;
            case LogVerbosity.Info:    _logger.Information(msg); break;
            case LogVerbosity.Warning: _logger.Warning(msg);     break;
            case LogVerbosity.Error:   _logger.Error(msg);       break;
        }
    }
}