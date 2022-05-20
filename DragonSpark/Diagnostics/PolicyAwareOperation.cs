using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using Polly;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Diagnostics;

public class PolicyAwareOperation<T> : IOperation<T>
{
	readonly IOperation<T> _previous;
	readonly IAsyncPolicy  _policy;

	protected PolicyAwareOperation(IOperation<T> previous, IResult<IAsyncPolicy> policy)
		: this(previous, policy.Get()) {}

	protected PolicyAwareOperation(IOperation<T> previous, IAsyncPolicy policy)
	{
		_previous = previous;
		_policy   = policy;
	}

	public ValueTask Get(T parameter)
		=> _policy.ExecuteAsync(new FixedSelection<T, ValueTask>(_previous, parameter).Allocate).ToOperation();
}

public class PolicyAwareOperation : IOperation
{
	readonly Func<Task>   _previous;
	readonly IAsyncPolicy _policy;

	protected PolicyAwareOperation(IOperation previous, IResult<IAsyncPolicy> policy) : this(previous, policy.Get()) {}

	protected PolicyAwareOperation(IOperation previous, IAsyncPolicy policy) : this(previous.Allocate, policy) {}

	protected PolicyAwareOperation(Func<ValueTask> previous, IResult<IAsyncPolicy> policy) 
		: this(previous.Allocate, policy.Get()) {}

	protected PolicyAwareOperation(Func<ValueTask> previous, IAsyncPolicy policy) : this(previous.Allocate, policy) {}

	protected PolicyAwareOperation(Func<Task> previous, IAsyncPolicy policy)
	{
		_previous = previous;
		_policy   = policy;
	}

	public ValueTask Get() => _policy.ExecuteAsync(_previous).ToOperation();
}