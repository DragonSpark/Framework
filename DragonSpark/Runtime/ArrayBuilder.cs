using DragonSpark.Model.Sequences;
using DragonSpark.Model.Sequences.Memory;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using Array = System.Array;

namespace DragonSpark.Runtime;

/// <summary>
/// Helper type for avoiding allocations while building arrays.
/// ATTRIBUTION: https://github.com/NetFabric/NetFabric.Hyperlinq/blob/7c971368b925cb9c4e687bf94c8314c4178d4410/NetFabric.Hyperlinq/Utils/ArrayBuilder/ArrayBuilder.cs
/// </summary>
/// <typeparam name="T">The element type.</typeparam>
public struct ArrayBuilder<T> : IDisposable
{
	public static implicit operator Array<T>(ArrayBuilder<T> instance) => instance.AsSpan().ToArray();

	const int defaultMinCapacity    = 4;
	const int maxCoreClrArrayLength = 0x7fefffff; // For byte arrays the limit is slightly larger

	readonly ArrayPool<T> pool;
	T[]?                  buffer; // Starts out null, initialized on first Add.

	public ArrayBuilder(int capacity) : this(capacity, ArrayPool<T>.Shared) {}

	public ArrayBuilder(int capacity, ArrayPool<T> pool) : this(pool)
	{
		Debug.Assert(capacity >= 0);
		if (capacity > 0)
		{
			buffer = this.pool.Rent(capacity);
		}
	}

	public ArrayBuilder(ArrayPool<T> pool)
	{
		this.pool = pool;
		buffer    = default;
		Count     = 0;
	}

	/// <summary>
	/// Gets the number of items this instance can store without re-allocating,
	/// or 0 if the backing array is <c>null</c>.
	/// </summary>
	public int Capacity => buffer?.Length ?? 0;

	/// <summary>
	/// Gets the number of items in the array currently in use.
	/// </summary>
	public int Count { get; private set; }

	// /// <summary>
	// /// Gets or sets the item at a certain index in the array.
	// /// </summary>
	// /// <param name="index">The index into the array.</param>
	// public T this[int index]
	// {
	//     get
	//     {
	//         Debug.Assert(index >= 0 && index < Count);
	//         return buffer![index];
	//     }
	// }

	public readonly Span<T> AsSpan() => buffer!.AsSpan(..Count);

	public readonly Leasing<T> AsLease()
	{
		if (Count == 0)
		{
			return Leasing<T>.Default;
		}

		var result = NewLeasing<T>.Default.Get((uint)Count);
		AsSpan().CopyTo(result.AsSpan());
		return result;
	}

	/// <summary>
	/// Adds an item to the backing array, resizing it if necessary.
	/// </summary>
	/// <param name="item">The item to add.</param>
	public void Add(T item)
	{
		if (Count == Capacity)
		{
			EnsureCapacity(Count + 1);
		}

		UncheckedAdd(item);
	}

	public void Add(ArrayBuilder<T> others)
	{
		var total = Count + others.Count;
		if (total >= Capacity)
		{
			EnsureCapacity(total);
		}
		Array.Copy(others.buffer!, buffer!, Count);
		Count = total;
	}

	public void Add(ICollection<T> others)
	{
		var total = Count + others.Count;
		if (total >= Capacity)
		{
			EnsureCapacity(total);
		}
		others.CopyTo(buffer!, Count);
		Count = total;
	}

	public void Add(Memory<T> others)
	{
		var total = Count + others.Length;
		if (total >= Capacity)
		{
			EnsureCapacity(total);
		}

		if (buffer is not null)
		{
			others.Span.CopyTo(buffer[Count..]);
		}

		Count = total;
	}

	/// <summary>
	/// Adds an item to the backing array, without checking if there is room.
	/// </summary>
	/// <param name="item">The item to add.</param>
	/// <remarks>
	/// Use this method if you know there is enough space in the <see cref="ArrayBuilder{T}"/>
	/// for another item, and you are writing performance-sensitive code.
	/// </remarks>
	public void UncheckedAdd(T item)
	{
		buffer![Count++] = item;
	}

	void EnsureCapacity(int minimum)
	{
		Debug.Assert(minimum > Capacity);

		var capacity = Capacity;
		var nextCapacity = capacity switch
		{
			0 => defaultMinCapacity,
			_ => 2 * capacity,
		};

		if (nextCapacity > maxCoreClrArrayLength)
		{
			nextCapacity = Math.Max(capacity + 1, maxCoreClrArrayLength);
		}

		nextCapacity = Math.Max(nextCapacity, minimum);

		var next = pool.Rent(nextCapacity);
		try
		{
			if (buffer is not null)
			{
				Array.Copy(buffer, next, Count);
			}
		}
		finally
		{
			if (buffer is not null)
				pool.Return(buffer);
			buffer = next;
		}
	}

	public readonly void Dispose()
	{
		if (buffer is not null)
		{
			pool.Return(buffer);
		}
	}
}

public static class ArrayBuilder
{
	public static ArrayBuilder<T> New<T>(uint count) => new((int)count);
	public static ArrayBuilder<T> New<T>(int count) => new(count);
}