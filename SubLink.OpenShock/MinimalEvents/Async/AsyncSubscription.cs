using System;
using System.Threading.Tasks;

namespace OpenShock.MinimalEvents.Async;

internal sealed class AsyncSubscription<T> : IAsyncDisposable
{
    private readonly T _handler;
    private readonly AsyncInvocationHandlerList<T> _handlerList;

    public AsyncSubscription(T handler, AsyncInvocationHandlerList<T> handlerList)
    {
        _handler = handler;
        _handlerList = handlerList;
    }

    public ValueTask DisposeAsync()
    {
        return _handlerList.Remove(_handler);
    }
}