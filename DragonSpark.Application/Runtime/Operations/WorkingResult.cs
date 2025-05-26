using DragonSpark.Compose;
using DragonSpark.Model.Operations.Results;
using JetBrains.Annotations;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Runtime.Operations;

public sealed class WorkingResult<T> : IWorkingResult<T>
{
    readonly IResulting<T> _previous;
    readonly Func<Task> _complete;

    public WorkingResult(IResulting<T> previous, Func<Task> complete)
    {
        _previous = previous;
        _complete = complete;
    }

    [MustDisposeResource]
    public Worker<T> Get()
    {
        var previous = _previous.Get();
        var task = previous.AsTask();
        if (previous.IsCompletedSuccessfully)
        {
            return new(Task.CompletedTask, task);
        }

        var source = new TaskCompletionSource<T>();
        var worker = new WorkerOperation<T>(task, source, _complete).Allocate();
        return new(worker, source.Task);
    }
}
