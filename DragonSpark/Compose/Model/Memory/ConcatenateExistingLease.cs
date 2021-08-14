using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences.Memory;
using System;

namespace DragonSpark.Compose.Model.Memory
{
	sealed class ConcatenateExistingLease<T> : ISelect<(Lease<T> First, Memory<T> Second), Concatenation<T>>
	{
		public static ConcatenateExistingLease<T> Default { get; } = new ConcatenateExistingLease<T>();

		ConcatenateExistingLease() {}

		public Concatenation<T> Get((Lease<T> First, Memory<T> Second) parameter)
		{
			var (first, second) = parameter;

			var span = second.Span;
			span.CopyTo(first.Remaining.Span);

			return new Concatenation<T>(first, first.Size((uint)(first.Length + span.Length)));
		}
	}
}