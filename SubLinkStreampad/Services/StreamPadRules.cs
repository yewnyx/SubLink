using System;
using System.Threading.Tasks;

namespace xyz.yewnyx.SubLink;

internal sealed class StreamPadRules : IStreamPadRules {
    internal Func<string, float, Task>? OnControllerValue;

    void IStreamPadRules.ReactToControllerValue(Func<string, float, Task> with) { OnControllerValue = with; }
}