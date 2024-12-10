using System;
using DragonSpark.Model.Sequences.Memory;
using JetBrains.Annotations;

namespace DragonSpark.Compose.Model.Memory;

public readonly struct Concatenation<T> : IDisposable
{
    readonly Leasing<T> _first;

    [MustDisposeResource]
    public Concatenation(Leasing<T> first, Leasing<T> instance)
    {
        _first = first;
        Instance = instance;
    }

    [MustDisposeResource]
    public Leasing<T> Result()
    {
        _first.Dispose();
        return Instance;
    }

    public Leasing<T> Instance { get; }

    public void Dispose()
    {
        var first = _first;
        first.Dispose();

        var result = Instance;
        result.Dispose();
    }
}
