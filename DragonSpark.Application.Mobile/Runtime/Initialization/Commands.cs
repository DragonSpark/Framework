using DragonSpark.Model;
using DragonSpark.Model.Results;
using NetFabric.Hyperlinq;
using System.Buffers;

namespace DragonSpark.Application.Mobile.Runtime.Initialization;

sealed class Commands : Commands<Action>
{
    public static Commands Default { get; } = new();

    Commands() : base(x => x()) {}
}

class Commands<T> : Instance<List<T>>, ICommands<T>
{
    readonly List<T>   _queue;
    readonly Action<T> _execute;

    protected Commands(Action<T> execute) : this([], execute) {}

    protected Commands(List<T> queue, Action<T> execute) : base(queue)
    {
        _queue = queue;
        _execute  = execute;
    }

    public void Execute(None parameter)
    {
        while (_queue.Count > 0)
        {
            using var lease = _queue.AsValueEnumerable().ToArray(ArrayPool<T>.Shared);
            foreach (var item in lease)
            {
                _execute(item);
                _queue.Remove(item);
            }
        }
    }
}