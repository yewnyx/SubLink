using System.Reflection;
using JetBrains.Annotations;
using Serilog;
using VRC.OSCQuery;
using static VRC.OSCQuery.Extensions;

namespace xyz.yewnyx.SubLink; 

[UsedImplicitly]
public sealed class OSCSupportService {
    public int OSCPort { get; private set; }
    public int OSCQueryPort { get; private set; }
    public OSCQueryService OSCQuery { get; private set; } = null!;

    private readonly ILogger _logger;
    private readonly MeaModDiscovery _discovery;

    public OSCSupportService(ILogger logger) {
        _logger = logger;
        _discovery = new MeaModDiscovery();
    }
    
    public void Start() {
        OSCPort = GetAvailableUdpPort();
        OSCQueryPort = GetAvailableTcpPort();

        _logger.Debug("Setting up OSCQuery on ports UDP:{OSCPort} TCP:{OSCQueryPort}", OSCPort, OSCQueryPort);
        
        _discovery.OnOscServiceAdded += profile => {
            _logger.Information("OSC Service Added: {Name} on port {Port}", profile.name, profile.port);
        };

        OSCQuery = new OSCQueryServiceBuilder()
            .WithServiceName("SubLink")
            .WithUdpPort(OSCPort)
            .WithTcpPort(OSCQueryPort)
            .WithDiscovery(_discovery)
            .StartHttpServer()
            .AdvertiseOSC()
            .AdvertiseOSCQuery()
            .Build();
        OSCQuery.RefreshServices();

        CommonGlobals.oscQuery = OSCQuery;
        // OSCQuery.AddEndpoint<bool>(..., Attributes.AccessValues.ReadWrite, new object[] { true }); 
    }
}