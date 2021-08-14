using DragonSpark.Model.Sequences.Memory;
using NetFabric.Hyperlinq;
using System;

namespace DragonSpark.Compose.Model.Memory
{
	public readonly struct LeaseSelector<T>
	{
		readonly Lease<T> _subject;

		public LeaseSelector(Lease<T> subject) => _subject = subject;

		public Concatenation<T> Concat(EnumerableExtensions.ValueEnumerable<T> memory)
			=> Concatenate<T>.Default.Get(_subject, memory);

		public Concatenation<T> Concat(Memory<T> memory) => ConcatenateLeases<T>.Default.Get(_subject, memory);

		public Lease<TTo> OfType<TTo>() => OfType<T, TTo>.Default.Get(_subject);
	}
}