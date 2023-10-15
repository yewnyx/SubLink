using BuildSoft.OscCore;
using JetBrains.Annotations;
using Serilog;
using VRC.OSCQuery;

namespace xyz.yewnyx;

[PublicAPI]
public interface IGlobals {
    public ILogger logger { get; set; }
    public IServiceProvider serviceProvider { get; set; }
    public OSCQueryService oscQuery { get; set; }
    public OscServer oscServer { get; set; }
}