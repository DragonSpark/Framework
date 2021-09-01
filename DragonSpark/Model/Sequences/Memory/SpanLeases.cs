using NetFabric.Hyperlinq;
using System.Buffers;

namespace DragonSpark.Model.Sequences.Memory
{
	sealed class EnumerableListLease<T> : ILease<ArrayExtensions.ArraySegmentValueEnumerable<T>, T>
	{
		public static EnumerableListLease<T> Default { get; } = new();

		EnumerableListLease() : this(ArrayPool<T>.Shared) {}

		readonly ArrayPool<T> _pool;

		public EnumerableListLease(ArrayPool<T> pool) => _pool = pool;

		public Lease<T> Get(ArrayExtensions.ArraySegmentValueEnumerable<T> parameter) => new(parameter.ToArray(_pool));
	}

	sealed class SpanLeases<T>
	{
		public static SpanLeases<T> Default { get; } = new();

		SpanLeases() : this(ArrayPool<T>.Shared) {}

		readonly ArrayPool<T> _pool;

		public SpanLeases(ArrayPool<T> pool) => _pool = pool;

		public Lease<T> Get(ArrayExtensions.SpanValueEnumerable<T> parameter) => new(parameter.ToArray(_pool));
	}
}