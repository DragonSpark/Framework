using System;
using System.Threading;
using DragonSpark.Composition;
using JetBrains.Annotations;

namespace DragonSpark.Application.Runtime.Operations;

[MustDisposeResource]
sealed class ScopedToken : IScopedToken, IDisposable
{
    readonly CancellationTokenSource _source;
    readonly CancellationToken _token;

    public ScopedToken(CancellationTokenSource source) : this(source, source.Token) { }

    [Candidate(false)]
    public ScopedToken(CancellationTokenSource source, CancellationToken token)
    {
        _source = source;
        _token = token;
    }

    public CancellationToken Get()
    {
        _token.ThrowIfCancellationRequested();
        return _token;
    }

    public void Dispose()
    {
        _source.Cancel();
    }
}
