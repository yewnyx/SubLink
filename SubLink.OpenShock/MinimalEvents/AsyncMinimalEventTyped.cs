using OpenShock.MinimalEvents.Async;
using System;
using System.Threading.Tasks;

namespace OpenShock.MinimalEvents;

public sealed class AsyncMinimalEvent<T> : IAsyncMinimalEventObservable<T>
{
    private readonly AsyncInvocationHandlerList<Func<T, Task>> _handlerList = new();
    
    public async Task InvokeAsyncParallel(T eventArg)
    {
        var handlersCopy = _handlerList.Handlers;
        var taskList = new Task[handlersCopy.Length];
        
        for (var i = 0; i < handlersCopy.Length; i++)
        {
            taskList[i] = handlersCopy[i].Invoke(eventArg);
        }
        
        var task = Task.WhenAll(taskList);

        try
        {
            await task.ConfigureAwait(false);
        }
        catch (Exception)
        {
            if (task.Exception != null)
            {
                throw task.Exception;
            }

            throw;
        }
    }

    public async ValueTask<IAsyncDisposable> SubscribeAsync(Func<T, Task> handler)
    {
        await _handlerList.Add(handler).ConfigureAwait(false);
        return new AsyncSubscription<Func<T, Task>>(handler, _handlerList);
    }
}