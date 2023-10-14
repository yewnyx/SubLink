using JetBrains.Annotations;
using Serilog;
using VRC.OSCQuery;

namespace xyz.yewnyx;

public static class CommonGlobals {
    [UsedImplicitly] 
    public static ILogger logger { get; set; } = null!;

    [UsedImplicitly] 
    public static IServiceProvider serviceProvider { get; set; } = null!;
    
    [UsedImplicitly]
    public static OSCQueryService oscQuery { get; set; } = null!;
}