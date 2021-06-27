using System;
using System.Buffers;

namespace DragonSpark.Model.Sequences.Memory
{
	public readonly struct Lease<T> : IDisposable
	{
		readonly IMemoryOwner<T> _owner;
		readonly Memory<T>       _memory;

		public Lease(IMemoryOwner<T> owner, uint length) : this(owner, owner.Memory, length) {}

		public Lease(IMemoryOwner<T> owner, Memory<T> memory, uint length)
		{
			_owner  = owner;
			_memory = memory;
			Length  = length;
		}

		public uint Length { get; }

		public T this[int index]
		{
			get => _memory.Span[index];
			set => _memory.Span[index] = value;
		}

		public void Dispose()
		{
			_owner.Dispose();
		}
	}
}