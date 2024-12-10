using System.Buffers;
using JetBrains.Annotations;
using NetFabric.Hyperlinq;

namespace DragonSpark.Model.Sequences.Memory;

sealed class SpanLeases<T>
{
    public static SpanLeases<T> Default { get; } = new();

    SpanLeases() : this(ArrayPool<T>.Shared) { }

    readonly ArrayPool<T> _pool;

    public SpanLeases(ArrayPool<T> pool) => _pool = pool;

    [MustDisposeResource]
    public Leasing<T> Get(ArrayExtensions.SpanValueEnumerable<T> parameter) => new(parameter.ToArray(_pool));
}
