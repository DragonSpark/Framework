using System;
using System.Buffers;

namespace DragonSpark.Model.Sequences.Memory
{
	public readonly struct Lease<T> : IDisposable
	{
		public static Lease<T> Default { get; } = new Lease<T>(EmptyOwner<T>.Default, 0);

		readonly IMemoryOwner<T> _owner;
		readonly Memory<T>       _memory;

		public Lease(IMemoryOwner<T> owner) : this(owner, (uint)owner.Memory.Length) {}

		public Lease(IMemoryOwner<T> owner, uint length) : this(owner, owner.Memory, length) {}

		public Lease(IMemoryOwner<T> owner, Memory<T> memory, uint length)
		{
			_owner  = owner;
			_memory = memory;
			Length  = length;
		}

		public Lease<T> Size(int size) => new(_owner, _memory, (uint)size);

		public Lease<T> Size(uint size) => new(_owner, _memory, size);

		public Memory<T> AsMemory() => _memory[..(int)Length];

		public Span<T> AsSpan() => _memory.Span[..(int)Length];

		public uint Length { get; }

		public uint ActualLength => (uint)_memory.Length;

		public T this[int index]
		{
			get => _memory.Span[index];
			set => _memory.Span[index] = value;
		}

		public T this[uint index]
		{
			get => _memory.Span[(int)index];
			set => _memory.Span[(int)index] = value;
		}

		public void Dispose()
		{
			_owner.Dispose();
		}
	}
}