using BuildSoft.OscCore;
using JetBrains.Annotations;
using Serilog;
using System.Reflection;
using VRC.OSCQuery;
using xyz.yewnyx.SubLink.Platforms;

namespace xyz.yewnyx;

internal static class HostGlobals {
    public struct PlatformInfo {
        public Assembly Assembly { get; set; }
        public IPlatform Entry { get; set; }
    }

    public static Dictionary<string, PlatformInfo> Platforms { get; set; } = new();
}

[PublicAPI]
public class ScriptGlobals {
    [UsedImplicitly] public ILogger logger { get; set; } = null!;
    [UsedImplicitly] public IServiceProvider serviceProvider { get; set; } = null!;
    [UsedImplicitly] public OSCQueryService oscQuery { get; set; } = null!;
    [UsedImplicitly] public OscServer oscServer { get; set; } = null!;
    [UsedImplicitly] public Dictionary<string, IPlatformRules> rules { get; set; } = new();
}
