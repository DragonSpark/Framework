using NetFabric.Hyperlinq;
using System.Buffers;

namespace DragonSpark.Model.Sequences.Memory
{
	public sealed class Leases<T> : ILeases<T>
	{
		public static Leases<T> Default { get; } = new Leases<T>();

		Leases() : this(ArrayPool<T>.Shared) {}

		readonly ArrayPool<T> _pool;

		public Leases(ArrayPool<T> pool) => _pool = pool;

		public Lease<T> Get(uint parameter) => new(_pool.RentSliced((int)parameter, false), parameter);
	}
}