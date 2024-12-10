using System;
using DragonSpark.Model.Sequences.Memory;
using JetBrains.Annotations;

namespace DragonSpark.Compose.Model.Memory;

sealed class ConcatenateMemory<T> : ILease<(Memory<T> First, Memory<T> Second), T>
{
    public static ConcatenateMemory<T> Default { get; } = new();

    ConcatenateMemory() : this(ConcatenateNewMemory<T>.Default) { }

    readonly ConcatenateNewMemory<T> _new;

    public ConcatenateMemory(ConcatenateNewMemory<T> @new) => _new = @new;

    [MustDisposeResource]
    public Leasing<T> Get((Memory<T> First, Memory<T> Second) parameter)
    {
        var (first, second) = parameter;
        var size = (uint)(first.Length + second.Length);
        var result = _new.Get(first, second, size);
        return result;
    }
}
