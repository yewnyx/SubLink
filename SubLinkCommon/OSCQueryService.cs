using BuildSoft.OscCore;
using JetBrains.Annotations;
using Serilog;
using VRC.OSCQuery;
using static VRC.OSCQuery.Extensions;

namespace xyz.yewnyx.SubLink; 

[UsedImplicitly]
public sealed class OSCSupportService<TGlobals> where TGlobals : IGlobals {
    public int OSCPort { get; private set; }
    public int OSCQueryPort { get; private set; }

    private readonly ILogger _logger;
    private readonly TGlobals _globals;
    private readonly MeaModDiscovery _discovery;

    public OSCSupportService(ILogger logger, TGlobals globals) {
        _logger = logger;
        _globals = globals;
        _discovery = new();
    }
    
    public void Start() {
        OSCPort = GetAvailableUdpPort();
        OSCQueryPort = GetAvailableTcpPort();

        _logger.Debug("Setting up OSCQuery on ports UDP:{OSCPort} TCP:{OSCQueryPort}", OSCPort, OSCQueryPort);
        
        _discovery.OnOscServiceAdded += profile => {
            _logger.Information("OSC Service Added: {Name} on port {Port}", profile.name, profile.port);
        };

        _globals.oscServer = OscServer.GetOrCreate(OSCPort);

        _globals.oscQuery = new OSCQueryServiceBuilder()
            .WithServiceName("SubLink")
            .WithUdpPort(OSCPort)
            .WithTcpPort(OSCQueryPort)
            .WithDiscovery(_discovery)
            .StartHttpServer()
            .AdvertiseOSC()
            .AdvertiseOSCQuery()
            .Build();
        _globals.oscQuery.RefreshServices();

        /*
        CommonGlobals.oscQuery.AddEndpoint<bool>("/avatar/parameters/MuteSelf", Attributes.AccessValues.ReadWrite, new object[] { true }); 
        CommonGlobals.oscServer.TryAddMethod("/avatar/parameters/MuteSelf", message => {
            _logger.Information($"Received `/avatar/parameters/MuteSelf` value from VRChat : {message.ReadBooleanElement(0)}");
        });
        */
    }
}