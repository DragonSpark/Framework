using DragonSpark.Compose;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations;

sealed class TokenAware<TIn, TOut> : ISelecting<TIn, TOut>
{
	readonly ISelect<TIn, ValueTask<TOut>> _previous;
	readonly CancellationToken             _token;
	readonly IMutable<CancellationToken>   _store;

	public TokenAware(ISelect<TIn, ValueTask<TOut>> previous, CancellationToken token)
		: this(previous, token, AmbientToken.Default) {}

	public TokenAware(ISelect<TIn, ValueTask<TOut>> previous, CancellationToken token,
	                  IMutable<CancellationToken> store)
	{
		_previous = previous;
		_token    = token;
		_store    = store;
	}

	public async ValueTask<TOut> Get(TIn parameter)
	{
		using (_store.Assigned(_token))
		{
			return await _previous.Await(parameter);
		}
	}
}