using DragonSpark.Compose;
using DragonSpark.Model.Results;
using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Application.Runtime.Operations;

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

	public async ValueTask Get()
	{
		if (_store.TryPop(out var source) && source is not null)
		{
			await source.CancelAsync().Off();
		}
	}

	public CancellationToken Token => _source.Get().Token;
}