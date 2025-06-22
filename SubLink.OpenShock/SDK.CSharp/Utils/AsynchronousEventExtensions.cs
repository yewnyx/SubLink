using System;
using System.Linq;
using System.Threading.Tasks;

namespace OpenShock.SDK.CSharp.Utils;

public static class AsynchronousEventExtensions
{
    public static Task Raise(this Func<Task>? handlers)
    {
        if (handlers != null)
        {
            return Task.WhenAll(handlers.GetInvocationList()
                .OfType<Func<Task>>()
                .Select(h => h()));
        }

        return Task.CompletedTask;
    }
    
    public static Task Raise<T0>(this Func<T0, Task>? handlers, T0 t0)
    {
        if (handlers != null)
        {
            return Task.WhenAll(handlers.GetInvocationList()
                .OfType<Func<T0, Task>>()
                .Select(h => h(t0)));
        }

        return Task.CompletedTask;
    }
    
    public static Task Raise<T0, T1>(this Func<T0, T1, Task>? handlers, T0 t0, T1 t1)
    {
        if (handlers != null)
        {
            return Task.WhenAll(handlers.GetInvocationList()
                .OfType<Func<T0, T1, Task>>()
                .Select(h => h(t0, t1)));
        }

        return Task.CompletedTask;
    }
}