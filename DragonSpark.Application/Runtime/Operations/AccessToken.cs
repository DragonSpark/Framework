using DragonSpark.Composition;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Application.Runtime.Operations;

// TODO
public interface IScopedToken : IResult<CancellationToken>;

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

sealed class TokenSource : Stored<CancellationTokenSource>
{
	public TokenSource(IMutable<CancellationTokenSource?> store) : base(store, () => new()) {}
}

public interface ITokenHandle : IOperation
{
	CancellationToken Token { get; }
}

public sealed class TokenHandle : ITokenHandle
{
	readonly IMutable<CancellationTokenSource?> _store;
	readonly IResult<CancellationTokenSource>   _source;

	public TokenHandle() : this(new Variable<CancellationTokenSource>(new CancellationTokenSource())) {}

	public TokenHandle(IMutable<CancellationTokenSource?> store) : this(store, new TokenSource(store)) {}

	public TokenHandle(IMutable<CancellationTokenSource?> store, IResult<CancellationTokenSource> source)
	{
		_store  = store;
		_source = source;
	}

	/*public CancellationToken Get()
	{
		var source = _source.Get();
		if (source.IsCancellationRequested)
		{
			Execute(None.Default);
		}

		return source.Token;
	}

	public void Execute(None parameter)
	{
		_source.Get().CancelAsync();
		_store.Execute(null);
	}*/
	public async ValueTask Get()
	{
		var source = _store.Get();
		if (source is not null)
		{
			_store.Execute(null);
			await source.CancelAsync().ConfigureAwait(false);
		}
	}

	public CancellationToken Token => _source.Get().Token;
}