using DragonSpark.Model.Sequences.Memory;
using System;

namespace DragonSpark.Compose.Model.Memory
{
	sealed class ConcatenateNewLease<T> : ILease<(Lease<T> First, Memory<T> Second, uint size), T>
	{
		public static ConcatenateNewLease<T> Default { get; } = new();

		ConcatenateNewLease() : this(Leases<T>.Default) {}

		readonly ILeases<T> _leases;

		public ConcatenateNewLease(ILeases<T> leases) => _leases = leases;

		public Lease<T> Get((Lease<T> First, Memory<T> Second, uint size) parameter)
		{
			var (first, second, size) = parameter;

			var result      = _leases.Get(size);
			var destination = result.AsSpan();

			var one = first.AsSpan();
			one.CopyTo(destination[..one.Length]);
			second.Span.CopyTo(destination[one.Length..]);

			first.Dispose();
			return result;
		}
	}
}