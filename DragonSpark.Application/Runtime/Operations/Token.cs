using DragonSpark.Composition;
using DragonSpark.Model.Results;
using System;
using System.Threading;

namespace DragonSpark.Application.Runtime.Operations;
// TODO
public interface IToken : IResult<CancellationToken>;
sealed class Token : IToken, IDisposable
{
	readonly CancellationTokenSource _source;
	readonly CancellationToken       _token;

	public Token(CancellationTokenSource source) : this(source, source.Token) {}

	[Candidate(false)]
	public Token(CancellationTokenSource source, CancellationToken token)
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