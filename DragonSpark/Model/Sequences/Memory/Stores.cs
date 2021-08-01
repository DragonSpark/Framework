using DragonSpark.Model.Selection;
using System;
using System.Buffers;

namespace DragonSpark.Model.Sequences.Memory
{
	public sealed class Stores<T> : ISelect<Memory<T>, Store<T>>
	{
		public static Stores<T> Default { get; } = new Stores<T>();

		Stores() : this(ArrayPool<T>.Shared) {}

		readonly ArrayPool<T> _pool;

		public Stores(ArrayPool<T> pool) => _pool = pool;

		public Store<T> Get(Memory<T> parameter)
		{
			var elements = _pool.Rent(parameter.Length);
			parameter.CopyTo(elements);
			var result   = new Store<T>(elements, (uint)parameter.Length, _pool);
			return result;
		}
	}
}