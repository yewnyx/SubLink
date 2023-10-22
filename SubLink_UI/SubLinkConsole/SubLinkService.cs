using FlowGraph;
using FlowGraph.Logger;
using FlowGraph.Node.StandardEventNode;
using FlowGraph.Process;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.Xml;

namespace tech.sublink.SubLinkConsole;

internal class SubLinkService : BackgroundService {
    private const string sequenceName = "appelsap";
    private readonly static string sequenceFile = Path.GetFullPath($"../../../../../{sequenceName}.xml");

    private readonly ILogger _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ConsoleLogger _consoleLogger;

    public SubLinkService(ILogger logger, IServiceScopeFactory serviceScopeFactory, ConsoleLogger consoleLogger) {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
        _consoleLogger = consoleLogger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
        using var sublinkScope = _serviceScopeFactory.CreateScope();
        var provider = new PhysicalFileProvider(Directory.GetCurrentDirectory());
        var script = provider.GetFileInfo("SubLink.cs");

        try {
            LogManager.Instance.AddLogger(_consoleLogger);
            LogManager.Instance.Verbosity = LogVerbosity.Trace;
            ProcessLauncher.Instance.StartLoop();

            XmlDocument xmlDoc = new();
            xmlDoc.Load(sequenceFile);
            XmlNode? rootNode = xmlDoc.SelectSingleNode("SubLinkEditor");

            if (rootNode == null || rootNode.Attributes == null) {
                LogManager.Instance.WriteLine(LogVerbosity.Error, "Invalid XML document");
                return;
            }

            NamedVariableManager.Instance.Load(rootNode);
            GraphDataManager.Instance.Load(rootNode);

            _logger.Information("'{0}' successfully loaded", sequenceFile);

            Sequence seq = GraphDataManager.Instance.GraphList.First(g => g.Name.Equals(sequenceName));
            ProcessLauncher.Instance.LaunchSequence(seq, typeof(EventNodeTestStarted), 0, "test");

            while (!stoppingToken.IsCancellationRequested) {
                using var loopScope = _serviceScopeFactory.CreateScope();
                await Task.Delay(Timeout.InfiniteTimeSpan, stoppingToken);
            }
        } finally {
            ProcessLauncher.Instance.StopLoop();
        }
    }
}
