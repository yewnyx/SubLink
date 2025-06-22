using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;

namespace OpenShock.MinimalEvents.Async;

internal sealed class AsyncInvocationHandlerList<T>
{
    public ImmutableArray<T> Handlers { get; private set; } = ImmutableArray<T>.Empty;
    
    private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
    
    public async ValueTask Add(T handler)
    {
        await _semaphore.WaitAsync().ConfigureAwait(false);

        try
        {
            Handlers = Handlers.Add(handler);
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async ValueTask Remove(T handler)
    {
        await _semaphore.WaitAsync().ConfigureAwait(false);
        
        try
        {
            Handlers = Handlers.Remove(handler);
        }
        finally
        {
            _semaphore.Release();
        }
    }
}