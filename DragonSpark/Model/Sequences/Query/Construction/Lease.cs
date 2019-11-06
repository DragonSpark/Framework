using System.Buffers;

namespace DragonSpark.Model.Sequences.Query.Construction
{
	sealed class Lease<T> : IEnter<T>
	{
		public static Lease<T> Default { get; } = new Lease<T>();

		Lease() : this(ArrayPool<T>.Shared) {}

		readonly ArrayPool<T> _pool;

		public Lease(ArrayPool<T> pool) => _pool = pool;

		public Store<T> Get(T[] parameter)
			=> new Store<T>(parameter.CopyInto(_pool.Rent(parameter.Length)), (uint)parameter.Length);
	}
}