using DragonSpark.Model.Sequences.Memory;
using NetFabric.Hyperlinq;
using System;

namespace DragonSpark.Compose.Model.Memory
{
	public readonly struct LeaseSelector<T>
	{
		readonly Lease<T> _subject;

		public LeaseSelector(Lease<T> subject) => _subject = subject;

		public Lease<T> Concat(EnumerableExtensions.ValueEnumerable<T> memory)
			=> Concatenate<T>.Default.Get(_subject, memory);

		public Lease<T> Concat(Memory<T> memory) => ConcatenateMemory<T>.Default.Get(_subject, memory);

		public Lease<TTo> OfType<TTo>() => OfType<T, TTo>.Default.Get(_subject);
	}
}