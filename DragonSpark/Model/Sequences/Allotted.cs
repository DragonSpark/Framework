using System.Buffers;

namespace DragonSpark.Model.Sequences
{
	public sealed class Allotted<T> : IStores<T>
	{
		public static Allotted<T> Default { get; } = new Allotted<T>();

		Allotted() : this(ArrayPool<T>.Shared) {}

		readonly ArrayPool<T> _pool;

		public Allotted(ArrayPool<T> pool) => _pool = pool;

		public Store<T> Get(uint parameter) => new Store<T>(_pool.Rent((int)parameter), parameter);
	}
}