using DragonSpark.Composition;
using System;
using System.Threading;

namespace DragonSpark.Application.Runtime.Operations;

sealed class ScopedToken : IScopedToken, IDisposable
{
	readonly CancellationTokenSource _source;
	readonly CancellationToken       _token;

	public ScopedToken(CancellationTokenSource source) : this(source, source.Token) {}

	[Candidate(false)]
	public ScopedToken(CancellationTokenSource source, CancellationToken token)
	{
		_source = source;
		_token  = token;
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