using System;
using System.Buffers;

namespace DragonSpark.Model.Sequences.Memory
{
	sealed class EmptyOwner<T> : IMemoryOwner<T>
	{
		public static EmptyOwner<T> Default { get; } = new EmptyOwner<T>();

		EmptyOwner() : this(new(Empty.Array<T>())) {}

		public EmptyOwner(Memory<T> memory) => Memory = memory;

		public Memory<T> Memory { get; }

		public void Dispose() {}
	}
}