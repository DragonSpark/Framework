using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using Polly;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Diagnostics;

public class PolicyAwareResulting<T> : IResulting<T>
{
	readonly Func<Task<T>>   _previous;
	readonly IAsyncPolicy<T> _policy;

	protected PolicyAwareResulting(IResulting<T> previous, IResult<IAsyncPolicy<T>> policy)
		: this(previous.Then().Allocate(), policy.Get()) {}

	protected PolicyAwareResulting(Func<Task<T>> previous, IAsyncPolicy<T> policy)
	{
		_previous = previous;
		_policy   = policy;
	}

	public ValueTask<T> Get() => _policy.ExecuteAsync(_previous).ToOperation();
}