using System;
using System.Threading;
using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Stop;
using DragonSpark.Model.Results;

namespace DragonSpark.Application.Mobile.Runtime.Initialization;

public sealed class TaskMonitor : IStopAware
{
    readonly IMutable<Task?> _current;
    readonly Func<Task>      _next;

    public TaskMonitor(Func<Task> next) : this(new Variable<Task>(next()), next) {}

    public TaskMonitor(IMutable<Task?> current, Func<Task> next)
    {
        _current = current;
        _next    = next;
    }

    public ValueTask Get(CancellationToken parameter)
    {
        var task = _current.TryPop(out var t) ? t : _next();
        return task.ToOperation();
    }
}