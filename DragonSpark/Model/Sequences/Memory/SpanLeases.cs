using NetFabric.Hyperlinq;
using System.Buffers;

namespace DragonSpark.Model.Sequences.Memory;

sealed class SpanLeases<T>
{
	public static SpanLeases<T> Default { get; } = new();

	SpanLeases() : this(ArrayPool<T>.Shared) {}

	readonly ArrayPool<T> _pool;

	public SpanLeases(ArrayPool<T> pool) => _pool = pool;

	public Leasing<T> Get(ArrayExtensions.SpanValueEnumerable<T> parameter) => new(parameter.ToArray(_pool));
}