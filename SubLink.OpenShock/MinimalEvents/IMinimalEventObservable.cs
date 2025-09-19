using System;

namespace OpenShock.MinimalEvents;

public interface IMinimalEventObservable {
    public IDisposable Subscribe(Action handler);
}

public interface IMinimalEventObservable<out T> {
    public IDisposable Subscribe(Action<T> handler);
}