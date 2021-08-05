using DragonSpark.Model.Sequences.Memory;
using System;

namespace DragonSpark.Compose.Model.Memory
{
	sealed class ConcatenateMemory<T> : ILease<(Lease<T> First, Memory<T> Second), T>
	{
		public static ConcatenateMemory<T> Default { get; } = new ConcatenateMemory<T>();

		ConcatenateMemory() : this(ConcatenateNewMemory<T>.Default, ConcatenateExistingMemory<T>.Default) {}

		readonly ConcatenateNewMemory<T>      _new;
		readonly ConcatenateExistingMemory<T> _existing;

		public ConcatenateMemory(ConcatenateNewMemory<T> @new, ConcatenateExistingMemory<T> existing)
		{
			_new      = @new;
			_existing = existing;
		}

		public Lease<T> Get((Lease<T> First, Memory<T> Second) parameter)
		{
			var (first, second) = parameter;
			var size   = (uint)(first.Length + second.Length);
			var result = size <= first.ActualLength ? _existing.Get(first, second) : _new.Get(first, second, size);
			return result;
		}
	}
}