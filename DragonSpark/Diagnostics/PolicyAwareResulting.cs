using DragonSpark.Compose;
using DragonSpark.Model.Operations.Results.Stop;
using DragonSpark.Model.Results;
using Polly;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Diagnostics;

public class PolicyAwareResulting<T> : IStopAware<T>
{
	readonly Func<CancellationToken, Task<T>> _previous;
	readonly IAsyncPolicy<T>                  _policy;

	protected PolicyAwareResulting(IStopAware<T> previous, IResult<IAsyncPolicy<T>> policy)
		: this(previous.Then().Allocate(), policy.Get()) {}

	protected PolicyAwareResulting(Func<CancellationToken, Task<T>> previous, IAsyncPolicy<T> policy)
	{
		_previous = previous;
		_policy   = policy;
	}

	public ValueTask<T> Get(CancellationToken parameter) => _policy.ExecuteAsync(_previous, parameter).ToOperation();
}