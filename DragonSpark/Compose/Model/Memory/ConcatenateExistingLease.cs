using DragonSpark.Model.Sequences.Memory;
using System;

namespace DragonSpark.Compose.Model.Memory
{
	sealed class ConcatenateExistingLease<T> : ILease<(Lease<T> First, Memory<T> Second), T>
	{
		public static ConcatenateExistingLease<T> Default { get; } = new ConcatenateExistingLease<T>();

		ConcatenateExistingLease() {}

		public Lease<T> Get((Lease<T> First, Memory<T> Second) parameter)
		{
			var (first, second) = parameter;

			var span = second.Span;
			span.CopyTo(first.Remaining.Span);

			return first.Size((uint)(first.Length + span.Length));
		}
	}
}