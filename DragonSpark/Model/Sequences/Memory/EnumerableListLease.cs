using System.Buffers;
using JetBrains.Annotations;
using NetFabric.Hyperlinq;

namespace DragonSpark.Model.Sequences.Memory;

sealed class EnumerableListLease<T> : ILease<ArrayExtensions.ArraySegmentValueEnumerable<T>, T>
{
    public static EnumerableListLease<T> Default { get; } = new();

    EnumerableListLease() : this(ArrayPool<T>.Shared) { }

    readonly ArrayPool<T> _pool;

    public EnumerableListLease(ArrayPool<T> pool) => _pool = pool;

    [MustDisposeResource]
    public Leasing<T> Get(ArrayExtensions.ArraySegmentValueEnumerable<T> parameter) => new(parameter.ToArray(_pool));
}
