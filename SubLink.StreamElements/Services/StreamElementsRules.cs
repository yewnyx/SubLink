using JetBrains.Annotations;
using System;
using System.Threading.Tasks;
using xyz.yewnyx.SubLink.Platforms;
using xyz.yewnyx.SubLink.StreamElements.SEClient;

namespace xyz.yewnyx.SubLink.StreamElements.Services;

[PublicAPI]
public sealed class StreamElementsRules : IPlatformRules {
    internal Func<TipEventArgs, Task>? OnTipEvent;

    public void ReactToTipEvent(Func<TipEventArgs, Task> with) { OnTipEvent = with; }
}
