using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Compose.Model.Operations;

sealed class TokenAwareOperationResult<T> : IResulting<T>
{
	readonly IResult<ValueTask<T>>   _operation;
	readonly Func<CancellationToken> _token;

	public TokenAwareOperationResult(IResult<ValueTask<T>> operation, Func<CancellationToken> token)
	{
		_operation = operation;
		_token     = token;
	}

	public async ValueTask<T> Get()
	{
		var token = _token();
		token.ThrowIfCancellationRequested();
		var result = await _operation.Await();
		token.ThrowIfCancellationRequested();
		return result;
	}
}