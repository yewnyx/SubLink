using BuildSoft.OscCore;
using JetBrains.Annotations;
using Serilog;
using VRC.OSCQuery;
using static VRC.OSCQuery.Extensions;

namespace xyz.yewnyx.SubLink; 

[UsedImplicitly]
public sealed class OSCSupportService {
    public int OSCPort { get; private set; }
    public int OSCQueryPort { get; private set; }

    private readonly ILogger _logger;
    private readonly MeaModDiscovery _discovery;

    public OSCSupportService(ILogger logger) {
        _logger = logger;
        _discovery = new();
    }
    
    public void Start() {
        OSCPort = GetAvailableUdpPort();
        OSCQueryPort = GetAvailableTcpPort();

        _logger.Debug("Setting up OSCQuery on ports UDP:{OSCPort} TCP:{OSCQueryPort}", OSCPort, OSCQueryPort);
        
        _discovery.OnOscServiceAdded += profile => {
            _logger.Information("OSC Service Added: {Name} on port {Port}", profile.name, profile.port);
        };

        CommonGlobals.oscServer = OscServer.GetOrCreate(OSCPort);

        CommonGlobals.oscQuery = new OSCQueryServiceBuilder()
            .WithServiceName("SubLink")
            .WithUdpPort(OSCPort)
            .WithTcpPort(OSCQueryPort)
            .WithDiscovery(_discovery)
            .StartHttpServer()
            .AdvertiseOSC()
            .AdvertiseOSCQuery()
            .Build();
        CommonGlobals.oscQuery.RefreshServices();

        /*
        CommonGlobals.oscQuery.AddEndpoint<bool>("/avatar/parameters/MuteSelf", Attributes.AccessValues.ReadWrite, new object[] { true }); 
        CommonGlobals.oscServer.TryAddMethod("/avatar/parameters/MuteSelf", message => {
            _logger.Information($"Received `/avatar/parameters/MuteSelf` value from VRChat : {message.ReadBooleanElement(0)}");
        });
        */
    }
}