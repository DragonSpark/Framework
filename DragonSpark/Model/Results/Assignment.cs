using System;
using JetBrains.Annotations;

namespace DragonSpark.Model.Results;

public readonly struct Assignment<T> : IDisposable
{
    readonly IMutable<T?> _subject;

    [MustDisposeResource]
    public Assignment(IMutable<T?> subject) => _subject = subject;

    public void Dispose()
    {
        _subject.Execute(default);
    }
}
