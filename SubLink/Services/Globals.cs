using System;
using JetBrains.Annotations;
using Serilog;
using xyz.yewnyx.SubLink;

namespace xyz.yewnyx;

public static class Globals {
    [UsedImplicitly] 
    public static ILogger logger { get; internal set; } = null!;

    [UsedImplicitly] 
    public static IServiceProvider serviceProvider { get; internal set; } = null!;

    [UsedImplicitly] 
    public static IRules rules { get; internal set; } = null!;
}