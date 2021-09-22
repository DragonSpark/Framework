﻿using System;
using System.Buffers;

namespace DragonSpark.Model.Sequences.Memory
{
	public readonly struct Lease<T> : IDisposable
	{
		public static implicit operator Memory<T>(Lease<T> instance) => instance.AsMemory();

		public static Lease<T> Default { get; } = new(EmptyOwner<T>.Default, Memory<T>.Empty, 0);

		readonly IMemoryOwner<T> _owner;
		readonly Memory<T>       _reference;

		public Lease(IMemoryOwner<T> owner) : this(owner, (uint)owner.Memory.Length) {}

		public Lease(IMemoryOwner<T> owner, uint length) : this(owner, owner.Memory, length) {}

		public Lease(IMemoryOwner<T> owner, Memory<T> reference, uint length)
		{
			_owner     = owner;
			_reference = reference;
			Length     = length;
		}

		public Lease<T> Size(int size) => new(_owner, _reference, (uint)size);

		public Lease<T> Size(uint size) => new(_owner, _reference, size);

		public Memory<T> Reference => _reference;

		public Memory<T> Remaining => _reference[(int)Length..];

		public Memory<T> AsMemory() => _reference[..(int)Length];

		public Span<T> AsSpan() => _reference.Span[..(int)Length];

		public uint Length { get; }

		public uint ActualLength => (uint)_reference.Length;

		/*public T this[int index]
		{
			get => _memory.Span[index];
			set => _memory.Span[index] = value;
		}

		public T this[uint index]
		{
			get => _memory.Span[(int)index];
			set => _memory.Span[(int)index] = value;
		}*/

		public T[] ToArray()
		{
			var result = AsSpan().ToArray();
			Dispose();
			return result;
		}

		public void Dispose()
		{
			var owner = _owner;
			owner.Dispose();
		}
	}
}