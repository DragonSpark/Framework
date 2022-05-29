using System;

namespace DragonSpark.Compose.Model.Memory;

public readonly struct LeaseSelector<T>
{
	readonly DragonSpark.Model.Sequences.Memory.Leasing<T> _subject;

	public LeaseSelector(DragonSpark.Model.Sequences.Memory.Leasing<T> subject) => _subject = subject;

	/*public Concatenation<T> Concat(EnumerableExtensions.ValueEnumerable<T> memory)
		=> Concatenate<T>.Default.Get(_subject, memory);*/

	public Concatenation<T> Concat(Memory<T> memory) => ConcatenateLeases<T>.Default.Get(_subject, memory);

	public DragonSpark.Model.Sequences.Memory.Leasing<TTo> OfType<TTo>() => OfType<T, TTo>.Default.Get(_subject);
}