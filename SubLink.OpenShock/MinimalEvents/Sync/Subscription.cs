using System;

namespace OpenShock.MinimalEvents.Sync;

internal sealed class Subscription<T> : IDisposable
{
    private readonly T _handler;
    private readonly InvocationHandlerList<T> _handlerList;

    public Subscription(T handler, InvocationHandlerList<T> handlerList)
    {
        _handler = handler;
        _handlerList = handlerList;
    }

    public void Dispose()
    {
        _handlerList.Remove(_handler);
    }
}