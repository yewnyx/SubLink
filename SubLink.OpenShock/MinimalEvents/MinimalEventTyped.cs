using OpenShock.MinimalEvents.Sync;
using System;

namespace OpenShock.MinimalEvents;

public sealed class MinimalEvent<T> : IMinimalEventObservable<T> {
    private readonly InvocationHandlerList<Action<T>> _handlerList = new();
    
    public IDisposable Subscribe(Action<T> handler) {
        _handlerList.Add(handler);
        return new Subscription<Action<T>>(handler, _handlerList);
    }

    public void Invoke(T eventArg) {
        foreach (var handlerListHandler in _handlerList.Handlers) {
            handlerListHandler.Invoke(eventArg);
        }
    }
}