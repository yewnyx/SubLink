using System;

namespace OpenShock.MinimalEvents.Sync;

internal sealed class Subscription<T>(T handler, InvocationHandlerList<T> handlerList) : IDisposable {
    private readonly T _handler = handler;
    private readonly InvocationHandlerList<T> _handlerList = handlerList;

    public void Dispose() =>
        _handlerList.Remove(_handler);
}