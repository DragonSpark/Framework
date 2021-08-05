using DragonSpark.Compose;
using NetFabric.Hyperlinq;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Model.Sequences.Memory
{
	public readonly struct Lease<T> : IDisposable, IAsyncDisposable
	{
		public static Lease<T> Default { get; } = new(new ValueMemoryOwner<T>(), Memory<T>.Empty, 0);

		readonly ValueMemoryOwner<T> _owner;
		readonly Memory<T>       _memory;

		public Lease(ValueMemoryOwner<T> owner) : this(owner, (uint)owner.Memory.Length) {}

		public Lease(ValueMemoryOwner<T> owner, uint length) : this(owner, owner.Memory, length) {}

		public Lease(ValueMemoryOwner<T> owner, Memory<T> memory, uint length)
		{
			_owner  = owner;
			_memory = memory;
			Length  = length;
		}

		public Lease<T> Size(int size) => new(_owner, _memory, (uint)size);

		public Lease<T> Size(uint size) => new(_owner, _memory, size);

		public Memory<T> Memory => _memory;
		public Memory<T> Remaining => _memory[(int)Length..];

		public Memory<T> AsMemory() => _memory[..(int)Length];

		public Span<T> AsSpan() => _memory.Span[..(int)Length];

		public uint Length { get; }

		public uint ActualLength => (uint)_memory.Length;

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

		public ValueTask DisposeAsync()
		{
			Dispose();
			return Task.CompletedTask.ToOperation();
		}
	}
}