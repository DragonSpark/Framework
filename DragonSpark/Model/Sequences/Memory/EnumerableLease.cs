using NetFabric.Hyperlinq;

namespace DragonSpark.Model.Sequences.Memory
{
	sealed class EnumerableLease<T> : ILease<EnumerableExtensions.ValueEnumerableWrapper<T>, T>
	{
		public static EnumerableLease<T> Default { get; } = new EnumerableLease<T>();

		EnumerableLease() : this(Leases<T>.Default) {}

		readonly ILeases<T> _leases;

		public EnumerableLease(ILeases<T> leases) => _leases = leases;

		public Lease<T> Get(EnumerableExtensions.ValueEnumerableWrapper<T> parameter)
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