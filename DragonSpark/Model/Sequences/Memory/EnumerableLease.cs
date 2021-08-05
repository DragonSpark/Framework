using NetFabric.Hyperlinq;
using System.Buffers;

namespace DragonSpark.Model.Sequences.Memory
{
	sealed class EnumerableLease<T> : ILease<EnumerableExtensions.ValueEnumerable<T>, T>
	{
		public static EnumerableLease<T> Default { get; } = new EnumerableLease<T>();

		EnumerableLease() : this(ArrayPool<T>.Shared, 64) {}

		readonly ArrayPool<T> _leases;
		readonly uint         _capacity;

		public EnumerableLease(ArrayPool<T> leases, uint capacity)
		{
			_leases   = leases;
			_capacity = capacity;
		}

		public Lease<T> Get(EnumerableExtensions.ValueEnumerable<T> parameter)
		{
			/*var builder = ArrayBuilder.New<T>(_capacity);
			foreach (var element in parameter)
			{
				builder.Add(element);
			}

			return builder.AsLease()*/
			return new Lease<T>(parameter.ToArray(_leases));
			;
		}
	}
}