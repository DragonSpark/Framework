using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences.Memory;
using System;

namespace DragonSpark.Compose.Model.Memory
{
	sealed class ConcatenateNewLease<T> : ISelect<(Lease<T> First, Memory<T> Second, uint size), Concatenation<T>>
	{
		public static ConcatenateNewLease<T> Default { get; } = new();

		ConcatenateNewLease() : this(Leases<T>.Default) {}

		readonly ILeases<T> _leases;

		public ConcatenateNewLease(ILeases<T> leases) => _leases = leases;

		public Concatenation<T> Get((Lease<T> First, Memory<T> Second, uint size) parameter)
		{
			var (first, second, size) = parameter;

			var result      = _leases.Get(size);
			var destination = result.AsSpan();

			var one = first.AsSpan();
			one.CopyTo(destination[..one.Length]);
			second.Span.CopyTo(destination[one.Length..]);

			return new (first, result);
		}
	}
}