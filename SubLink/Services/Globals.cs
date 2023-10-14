using System;
using JetBrains.Annotations;
using Serilog;
using VRC.OSCQuery;
using xyz.yewnyx.SubLink;

namespace xyz.yewnyx;

public static class Globals {
    [UsedImplicitly] 
    public static ILogger logger { get; internal set; } = null!;

    [UsedImplicitly] 
    public static IServiceProvider serviceProvider { get; internal set; } = null!;

    [UsedImplicitly] 
    public static ITwitchRules twitch { get; internal set; } = null!;
    
    [UsedImplicitly]
    public static OSCQueryService oscQuery { get; internal set; } = null!;
}