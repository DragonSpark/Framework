using DragonSpark.Model.Sequences.Memory;
using System;

namespace DragonSpark.Compose.Model.Memory
{
	sealed class ConcatenateMemory<T> : ILease<(Memory<T> First, Memory<T> Second), T>
	{
		public static ConcatenateMemory<T> Default { get; } = new ConcatenateMemory<T>();

		ConcatenateMemory() : this(ConcatenateNewMemory<T>.Default) {}

		readonly ConcatenateNewMemory<T> _new;

		public ConcatenateMemory(ConcatenateNewMemory<T> @new) => _new = @new;

		public Lease<T> Get((Memory<T> First, Memory<T> Second) parameter)
		{
			var (first, second) = parameter;
			var size   = (uint)(first.Length + second.Length);
			var result = _new.Get(first, second, size);
			return result;
		}
	}
}