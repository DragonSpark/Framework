using DragonSpark.Model.Sequences.Memory;
using System;

namespace DragonSpark.Compose.Model.Memory
{
	public readonly struct LeaseSelector<T>
	{
		readonly Lease<T> _subject;

		public LeaseSelector(Lease<T> subject) => _subject = subject;

		public Lease<T> Concat(Memory<T> memory) => Concatenate<T>.Default.Get(_subject.AsMemory(), memory);

		public Lease<TTo> OfType<TTo>() => OfType<T, TTo>.Default.Get(_subject);
	}
}