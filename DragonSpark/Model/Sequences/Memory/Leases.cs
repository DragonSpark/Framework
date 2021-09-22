using System.Buffers;

namespace DragonSpark.Model.Sequences.Memory
{
	public sealed class Leases<T> : ILeases<T>
	{
		public static Leases<T> Default { get; } = new Leases<T>();

		Leases() : this(MemoryPool<T>.Shared) {}

		readonly MemoryPool<T> _pool;

		public Leases(MemoryPool<T> pool) => _pool = pool;

		public Lease<T> Get(uint parameter) => new(_pool.Rent((int)parameter), parameter);
	}
}