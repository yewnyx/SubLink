using OpenShock.MinimalEvents.Async;
using System;
using System.Threading.Tasks;

namespace OpenShock.MinimalEvents;

public sealed class AsyncMinimalEvent : IAsyncMinimalEventObservable
{
    private readonly AsyncInvocationHandlerList<Func<Task>> _handlerList = new();
    
    public async Task InvokeAsyncParallel()
    {
        var handlersCopy = _handlerList.Handlers;
        var taskList = new Task[handlersCopy.Length];
        
        for (var i = 0; i < handlersCopy.Length; i++)
        {
            taskList[i] = handlersCopy[i].Invoke();
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

    public async ValueTask<IAsyncDisposable> SubscribeAsync(Func<Task> handler)
    {
        await _handlerList.Add(handler).ConfigureAwait(false);
        return new AsyncSubscription<Func<Task>>(handler, _handlerList);
    }
}