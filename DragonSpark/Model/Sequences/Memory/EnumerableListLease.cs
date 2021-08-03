using NetFabric.Hyperlinq;

namespace DragonSpark.Model.Sequences.Memory
{
	sealed class EnumerableListLease<T> : ILease<ReadOnlyListExtensions.ValueEnumerableWrapper<T>, T>
	{
		public static EnumerableListLease<T> Default { get; } = new();

		EnumerableListLease() : this(Leases<T>.Default) {}

		readonly ILeases<T> _leases;

		public EnumerableListLease(ILeases<T> leases) => _leases = leases;

		public Lease<T> Get(ReadOnlyListExtensions.ValueEnumerableWrapper<T> parameter)
		{
			var       count      = (uint)parameter.Count();
			var       result     = _leases.Get(count);
			var       span       = result.AsSpan();
			using var enumerator = parameter.GetEnumerator();
			var       i          = 0;
			while (enumerator.MoveNext())
			{
				span[i++] = enumerator.Current!;
			}
			return result;
		}
	}
}