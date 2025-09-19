using OpenShock.MinimalEvents.Sync;
using System;

namespace OpenShock.MinimalEvents;

public sealed class MinimalEvent : IMinimalEventObservable {
    private readonly InvocationHandlerList<Action> _handlerList = new();
    
    public IDisposable Subscribe(Action handler) {
        _handlerList.Add(handler);
        return new Subscription<Action>(handler, _handlerList);
    }

    public void Invoke() {
        foreach (var handlerListHandler in _handlerList.Handlers) {
            handlerListHandler.Invoke();
        }
    }
}