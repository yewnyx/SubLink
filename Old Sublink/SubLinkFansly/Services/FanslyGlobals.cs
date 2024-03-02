using System;
using BuildSoft.OscCore;
using JetBrains.Annotations;
using Serilog;
using VRC.OSCQuery;
using xyz.yewnyx.SubLink;

namespace xyz.yewnyx;

public class FanslyGlobals : IGlobals {
    [UsedImplicitly] public ILogger logger { get; set; } = null!;
    [UsedImplicitly] public IServiceProvider serviceProvider { get; set; } = null!;
    [UsedImplicitly] public OSCQueryService oscQuery { get; set; } = null!;
    [UsedImplicitly] public OscServer oscServer { get; set; } = null!;
    [UsedImplicitly] public IFanslyRules fansly { get; internal set; } = null!;
}