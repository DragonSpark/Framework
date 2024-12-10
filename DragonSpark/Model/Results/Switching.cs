using System;
using DragonSpark.Compose;
using JetBrains.Annotations;

namespace DragonSpark.Model.Results;

public readonly struct Switching : IDisposable
{
    readonly ISwitch _subject;

    [MustDisposeResource]
    public Switching(ISwitch subject) => _subject = subject;

    public void Dispose()
    {
        _subject.Down();
    }
}
