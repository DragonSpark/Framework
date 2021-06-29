using DragonSpark.Model.Selection.Alterations;
using NetFabric.Hyperlinq;
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


		public Lease<T> Size(int size) => new(_owner, _memory, (uint)size);
		public Lease<T> Size(uint size) => new(_owner, _memory, size);

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

	sealed class Distinct<T>// : IAlteration<Lease<T>>
	{
		public static Distinct<T> Default { get; } = new Distinct<T>();

		Distinct() {}

		public Lease<T> Get(in Lease<T> parameter)
		{
			var index = 0u;
			foreach (var element in parameter.AsSpan().Distinct())
			{
				parameter[index++] = element;
			}

			return parameter.Size(index);
		}
	}

	public static class LeaseExtensions
	{
		public static Lease<T> Distinct<T>(this in Lease<T> @this) => Memory.Distinct<T>.Default.Get(in @this);
	}
}