using System;
using System.Buffers;

namespace DragonSpark.Model.Sequences.Memory
{
	public readonly struct Store<T> : IDisposable
	{
		readonly ArrayPool<T> _pool;

		public Store(T[] elements, uint length, ArrayPool<T> pool)
		{
			_pool    = pool;
			Elements = elements;
			Length   = length;
		}

		public T[] Elements { get; }

		public uint Length { get; }

		public void Dispose()
		{
			_pool.Return(Elements);
		}

		public void Deconstruct(out T[] elements, out uint length)
		{
			elements = Elements;
			length   = Length;
		}
	}
}