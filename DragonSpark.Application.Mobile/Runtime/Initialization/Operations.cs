using System;
using System.Buffers;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Stop;
using DragonSpark.Model.Results;
using NetFabric.Hyperlinq;

namespace DragonSpark.Application.Mobile.Runtime.Initialization;

class Operations<T> : Instance<List<T>>, IOperations<T>
{
    readonly List<T>             _queue;
    readonly Func<Stop<T>, Task> _execute;

    protected Operations(Func<Stop<T>, Task> execute) : this([], execute) {}

    protected Operations(List<T> queue, Func<Stop<T>, Task> execute) : base(queue)
    {
        _queue   = queue;
        _execute = execute;
    }

    public async ValueTask Get(CancellationToken parameter)
    {
        while (_queue.Count > 0)
        {
            using var lease = _queue.AsValueEnumerable().ToArray(ArrayPool<T>.Shared);
            foreach (var item in lease)
            {
                await _execute(item.Stop(parameter)).Off();
                _queue.Remove(item);
            }
        }
    }
}

sealed class Operations : Operations<IStopAware>
{
    public static Operations Default { get; } = new();

    Operations() : base(x => x.Subject.Allocate(x.Token)) {}
}