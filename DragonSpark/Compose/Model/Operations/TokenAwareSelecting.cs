using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Compose.Model.Operations;

sealed class TokenAwareSelecting<TIn, TOut> : ISelecting<TIn, TOut>
{
	readonly ISelect<TIn, ValueTask<TOut>> _operation;
	readonly Func<CancellationToken>       _token;

	public TokenAwareSelecting(ISelect<TIn, ValueTask<TOut>> operation, Func<CancellationToken> token)
	{
		_operation = operation;
		_token     = token;
	}

	public async ValueTask<TOut> Get(TIn parameter)
	{
		var token = _token();
		token.ThrowIfCancellationRequested();
		var result = await _operation.Await(parameter);
		token.ThrowIfCancellationRequested();
		return result;
	}
}