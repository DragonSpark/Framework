using System;
using System.Threading;

namespace DragonSpark.Presentation.Runtime.Operations;

sealed class CircuitRequestToken(CancellationTokenSource source, CancellationToken token) : IRequestToken, IDisposable
{
    public CircuitRequestToken(CancellationTokenSource source) : this(source, source.Token) {}

    public CancellationToken Get()
    {
        token.ThrowIfCancellationRequested();
        return token;
    }

    public void Dispose()
    {
        source.Cancel();
    }
}