using System;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace xyz.yewnyx.SubLink; 

[PublicAPI]
public interface IStreamPadRules {
    public void ReactToControllerValue(Func<string, float, Task> with);
}