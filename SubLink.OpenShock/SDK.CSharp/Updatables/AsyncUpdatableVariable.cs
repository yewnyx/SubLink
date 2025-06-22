using OpenShock.MinimalEvents;
using System.Threading.Tasks;

namespace OpenShock.SDK.CSharp.Updatables;

public sealed class AsyncUpdatableVariable<T>(T internalValue) : IAsyncUpdatable<T>
{
    public T Value
    {
        get => _internalValue;
        set
        {
            if (_internalValue!.Equals(value)) return;
            _internalValue = value;
            Task.Run(() => _updated.InvokeAsyncParallel(value));
        }
    }

    public IAsyncMinimalEventObservable<T> Updated => _updated;
    private readonly AsyncMinimalEvent<T> _updated = new();
    private T _internalValue = internalValue;


    public void UpdateWithoutNotify(T newValue)
    {
        _internalValue = newValue;
    }
}