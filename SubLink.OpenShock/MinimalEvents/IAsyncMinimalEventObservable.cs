using System;
using System.Threading.Tasks;

namespace OpenShock.MinimalEvents;

public interface IAsyncMinimalEventObservable {
    public ValueTask<IAsyncDisposable> SubscribeAsync(Func<Task> handler);
}

public interface IAsyncMinimalEventObservable<out T> {
    public ValueTask<IAsyncDisposable> SubscribeAsync(Func<T, Task> handler);
}