using DragonSpark.Model.Sequences.Memory;
using System;

namespace DragonSpark.Compose.Model.Memory
{
	sealed class ConcatenateNewMemory<T> : ILease<(Memory<T> First, Memory<T> Second, uint size), T>
	{
		public static ConcatenateNewMemory<T> Default { get; } = new();

		ConcatenateNewMemory() : this(Leases<T>.Default) {}

		readonly ILeases<T> _leases;

		public ConcatenateNewMemory(ILeases<T> leases) => _leases = leases;

		public Lease<T> Get((Memory<T> First, Memory<T> Second, uint size) parameter)
		{
			var (first, second, size) = parameter;

			var result      = _leases.Get(size);
			var destination = result.AsSpan();

			var one = first.Span;
			one.CopyTo(destination[..one.Length]);
			second.Span.CopyTo(destination[one.Length..]);

			return result;
		}
	}
}