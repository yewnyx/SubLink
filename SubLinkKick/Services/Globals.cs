using JetBrains.Annotations;
using xyz.yewnyx.SubLink;

namespace xyz.yewnyx;

public static class Globals {
    [UsedImplicitly] 
    public static IKickRules kick { get; internal set; } = null!;
}