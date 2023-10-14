using JetBrains.Annotations;
using xyz.yewnyx.SubLink;

namespace xyz.yewnyx;

public static class Globals {
    [UsedImplicitly] 
    public static ITwitchRules twitch { get; internal set; } = null!;
}