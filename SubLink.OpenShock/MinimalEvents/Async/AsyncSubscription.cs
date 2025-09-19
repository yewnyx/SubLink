using System;
using System.Threading.Tasks;

namespace OpenShock.MinimalEvents.Async;

internal sealed class AsyncSubscription<T>(T handler, AsyncInvocationHandlerList<T> handlerList) : IAsyncDisposable {
    private readonly T _handler = handler;
    private readonly AsyncInvocationHandlerList<T> _handlerList = handlerList;

    public ValueTask DisposeAsync() =>
        _handlerList.Remove(_handler);
}