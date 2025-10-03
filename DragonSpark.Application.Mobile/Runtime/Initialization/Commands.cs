using System;
using System.Buffers;
using System.Collections.Generic;
using DragonSpark.Model;
using DragonSpark.Model.Results;
using NetFabric.Hyperlinq;

namespace DragonSpark.Application.Mobile.Runtime.Initialization;

sealed class Commands : Commands<Action>
{
    public static Commands Default { get; } = new();

    Commands() : base(x => x()) {}
}

class Commands<T> : ICommands<T>
{
    readonly IMutable<List<T>?> _previous;
    readonly Action<T>          _execute;

    protected Commands(Action<T> execute) : this(new Variable<List<T>>([]), execute) {}

    protected Commands(IMutable<List<T>?> previous, Action<T> execute)
    {
        _previous = previous;
        _execute  = execute;
    }

    public void Execute(None parameter)
    {
        var list = _previous.Get();
        if (list is not null)
        {
            using var lease = list.AsValueEnumerable().ToArray(ArrayPool<T>.Shared);
            foreach (var item in lease)
            {
                _execute(item);
                list.Remove(item);
            }

            _previous.Execute(null);
        }
    }

    public List<T>? Get() => _previous.Get();

    public void Execute(List<T>? parameter)
    {
        _previous.Execute(parameter);
    }
}