using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using xyz.yewnyx.SubLink.StreamElements;

namespace xyz.yewnyx.SubLink; 

[PublicAPI]
public interface IStreamElementsRules {
    public void ReactToTipEvent(Func<TipEventArgs, Task> with);
}