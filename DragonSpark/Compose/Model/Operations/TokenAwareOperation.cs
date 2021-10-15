using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Compose.Model.Operations;

sealed class TokenAwareOperation<T> : IOperation<T>
{
	readonly ISelect<T, ValueTask>   _operation;
	readonly Func<CancellationToken> _token;

	public TokenAwareOperation(ISelect<T, ValueTask> operation, Func<CancellationToken> token)
	{
		_operation = operation;
		_token     = token;
	}

	public async ValueTask Get(T parameter)
	{
		var token = _token();
		token.ThrowIfCancellationRequested();
		await _operation.Await(parameter);
		token.ThrowIfCancellationRequested();
	}
}