using System;
using System.Collections.Generic;
using DragonSpark.Model.Sequences.Memory;
using JetBrains.Annotations;

namespace DragonSpark.Compose.Model.Memory;

public readonly struct MemoryComposer<T>
{
    readonly Memory<T> _subject;

    public MemoryComposer(Memory<T> subject) => _subject = subject;

    [MustDisposeResource]
    public Leasing<T> Concat(Memory<T> memory) => ConcatenateMemory<T>.Default.Get(_subject, memory);

    public uint? IndexOf(T candidate) => IndexOf(candidate, EqualityComparer<T>.Default);

    public uint? IndexOf(T candidate, IEqualityComparer<T> comparer)
        => IndexOf<T>.Default.Get(_subject, candidate, comparer);

    public bool Contains(T candidate) => Contains(candidate, EqualityComparer<T>.Default);

    public bool Contains(T candidate, IEqualityComparer<T> comparer)
        => Contains<T>.Default.Get(_subject, candidate, comparer);

}
