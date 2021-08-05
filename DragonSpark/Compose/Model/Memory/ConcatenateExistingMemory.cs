using DragonSpark.Model.Sequences.Memory;
using System;

namespace DragonSpark.Compose.Model.Memory
{
	sealed class ConcatenateExistingMemory<T> : ILease<(Lease<T> First, Memory<T> Second), T>
	{
		public static ConcatenateExistingMemory<T> Default { get; } = new ConcatenateExistingMemory<T>();

		ConcatenateExistingMemory() {}

		public Lease<T> Get((Lease<T> First, Memory<T> Second) parameter)
		{
			var (first, second) = parameter;

			var span = second.Span;
			span.CopyTo(first.Remaining.Span);
			return first.Size((uint)(first.Length + span.Length));
		}
	}
}