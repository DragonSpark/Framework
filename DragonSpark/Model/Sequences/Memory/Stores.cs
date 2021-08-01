using DragonSpark.Model.Selection;
using NetFabric.Hyperlinq;
using System;
using System.Buffers;

namespace DragonSpark.Model.Sequences.Memory
{
	class Class1 {}

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

	sealed class StoredLeases<T> : ISelect<Lease<T>, StoredLease<T>>
	{
		public static StoredLeases<T> Default { get; } = new StoredLeases<T>();

		StoredLeases() : this(Stores<T>.Default) {}

		readonly Stores<T> _stores;

		public StoredLeases(Stores<T> stores) => _stores = stores;

		public StoredLease<T> Get(Lease<T> parameter) => new(parameter, _stores.Get(parameter.AsMemory()));
	}

	public readonly struct StoredLease<T> : IDisposable
	{
		public static implicit operator T[](StoredLease<T> source) => source.Store.Elements;
		public static implicit operator Memory<T>(StoredLease<T> source) => source.Lease.AsMemory();

		public StoredLease(Lease<T> lease, Store<T> store)
		{
			Lease = lease;
			Store = store;
		}

		public Lease<T> Lease { get; }

		public Store<T> Store { get; }

		public void Dispose()
		{
			Lease.Dispose();
			Store.Dispose();
		}
	}

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
			for (var i = 0; i < count; i++, enumerator.MoveNext())
			{
				span[i] = enumerator.Current!;
			}

			return default;
		}
	}
}