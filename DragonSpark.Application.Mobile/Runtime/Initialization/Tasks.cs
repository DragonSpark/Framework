using System;
using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;

namespace DragonSpark.Application.Mobile.Runtime.Initialization;

/*
sealed class Tasks : Operations<TaskMonitor>
{
    public static Tasks Default { get; } = new();

    Tasks() : base(x => x.Allocate()) {}
}
*/

public sealed class TaskMonitor : IOperation
{
    readonly IMutable<Task?> _current;
    readonly Func<Task>      _next;

    public TaskMonitor(Func<Task> next) : this(new Variable<Task>(next()), next) {}

    public TaskMonitor(IMutable<Task?> current, Func<Task> next)
    {
        _current = current;
        _next    = next;
    }

    public ValueTask Get()
    {
        var task = _current.TryPop(out var t) && t is not null ? t : _next();
        return task.ToOperation();
    }
}