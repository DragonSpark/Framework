using System;
using System.Threading;
using DragonSpark.Composition;
using JetBrains.Annotations;

namespace DragonSpark.Application.AspNet.Runtime.Operations;

[MustDisposeResource]
[method: Candidate(false)]
sealed class ScopedToken(CancellationTokenSource source, CancellationToken token) : IScopedToken, IDisposable
{
	public ScopedToken(CancellationTokenSource source) : this(source, source.Token) { }

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
