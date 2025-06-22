using OpenShock.MinimalEvents;

namespace OpenShock.SDK.CSharp.Updatables;

public interface IAsyncUpdatable<out T>
{
    public T Value { get; }
    public IAsyncMinimalEventObservable<T> Updated { get; }
}