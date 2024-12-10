using System;
using JetBrains.Annotations;

namespace DragonSpark.Compose.Model.Memory;

public readonly struct LeaseComposer<T>
{
    readonly DragonSpark.Model.Sequences.Memory.Leasing<T> _subject;

    public LeaseComposer(DragonSpark.Model.Sequences.Memory.Leasing<T> subject) => _subject = subject;

    /*public Concatenation<T> Concat(EnumerableExtensions.ValueEnumerable<T> memory)
		=> Concatenate<T>.Default.Get(_subject, memory);*/

    [MustDisposeResource(false)]
    public Concatenation<T> Concat(Memory<T> memory) => ConcatenateLeases<T>.Default.Get(_subject, memory);

    [MustDisposeResource(false)]
    public DragonSpark.Model.Sequences.Memory.Leasing<TTo> OfType<TTo>() => OfType<T, TTo>.Default.Get(_subject);
}
