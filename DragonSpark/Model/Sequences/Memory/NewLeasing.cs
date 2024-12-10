using System.Buffers;
using JetBrains.Annotations;
using NetFabric.Hyperlinq;

namespace DragonSpark.Model.Sequences.Memory;

public sealed class NewLeasing<T> : INewLeasing<T>
{
    public static NewLeasing<T> Default { get; } = new();

    NewLeasing() : this(ArrayPool<T>.Shared) { }

    readonly ArrayPool<T> _pool;

    public NewLeasing(ArrayPool<T> pool) => _pool = pool;

    [MustDisposeResource]
    public Leasing<T> Get(uint parameter) => new(_pool.Lease((int)parameter), parameter);
}
