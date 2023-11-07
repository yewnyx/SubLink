using System;
using System.Threading.Tasks;
using xyz.yewnyx.SubLink.StreamElements;

namespace xyz.yewnyx.SubLink;

internal sealed class StreamElementsRules : IStreamElementsRules {
    internal Func<TipEventArgs, Task>? OnTipEvent;

    void IStreamElementsRules.ReactToTipEvent(Func<TipEventArgs, Task> with) { OnTipEvent = with; }
}