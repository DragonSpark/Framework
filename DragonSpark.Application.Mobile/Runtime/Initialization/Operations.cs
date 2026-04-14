using System;
using System.Buffers;
using System.Collections.Generic;
using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using NetFabric.Hyperlinq;

namespace DragonSpark.Application.Mobile.Runtime.Initialization;

class Operations<T> : IOperations<T>
{
    readonly IMutable<List<T>?> _previous;
    readonly Func<T, Task>      _execute;

    protected Operations(Func<T, Task> execute) : this(new Variable<List<T>>([]), execute) {}

    protected Operations(IMutable<List<T>?> previous, Func<T, Task> execute)
    {
        _previous = previous;
        _execute  = execute;
    }

    public async ValueTask Get(None parameter)
    {
        if (_previous.TryPop(out var list) && list is not null)
        {
            using var lease = list.AsValueEnumerable().ToArray(ArrayPool<T>.Shared);
            foreach (var item in lease)
            {
                try
                {
                    await _execute(item).Off();
                }
                catch (Exception)
                {
                    _previous.Execute(list);
                    throw;
                }
                list.Remove(item);
            }
        }
    }

    public List<T>? Get() => _previous.Get();

    public void Execute(List<T>? parameter)
    {
        _previous.Execute(parameter);
    }
}

sealed class Operations : Operations<IOperation>
{
    public static Operations Default { get; } = new();

    Operations() : base(x => x.Allocate()) {}
}