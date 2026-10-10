using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Model.Operations.Stop;
using DragonSpark.Model.Results;
using Polly;

namespace DragonSpark.Diagnostics;

public class PolicyAware<T> : IStopAware<T>
{
	readonly IStopAware<T>  _previous;
	readonly IAsyncPolicy   _policy;
	readonly IMutable<bool> _scope;

	protected PolicyAware(IStopAware<T> previous, IResult<IAsyncPolicy> policy) : this(previous, policy.Get()) {}

	protected PolicyAware(IStopAware<T> previous, IAsyncPolicy policy)
		: this(previous, policy, CurrentPolicy.Default) {}

	protected PolicyAware(IStopAware<T> previous, IAsyncPolicy policy, IMutable<bool> scope)
	{
		_previous = previous;
		_policy   = policy;
		_scope    = scope;
	}

	public async ValueTask Get(Stop<T> parameter)
	{
		var (subject, stop) = parameter;

		using (_scope.Assigned(true))
		{
			await _policy.ExecuteAsync(x => _previous.Allocate(new(subject, x)), stop).Off();
		}
	}
}

public class PolicyAware : IStopAware
{
	readonly Func<CancellationToken, Task> _previous;
	readonly IAsyncPolicy                  _policy;
	readonly IMutable<bool>                _scope;

	protected PolicyAware(IStopAware previous, IResult<IAsyncPolicy> policy) : this(previous, policy.Get()) {}

	protected PolicyAware(IStopAware previous, IAsyncPolicy policy) : this(previous.Allocate, policy) {}

	protected PolicyAware(Func<CancellationToken, ValueTask> previous, IResult<IAsyncPolicy> policy)
		: this(previous.Allocate, policy.Get()) {}

	protected PolicyAware(Func<CancellationToken, ValueTask> previous, IAsyncPolicy policy)
		: this(previous.Allocate, policy) {}

	protected PolicyAware(Func<CancellationToken, Task> previous, IAsyncPolicy policy)
		: this(previous, policy, CurrentPolicy.Default) {}

	protected PolicyAware(Func<CancellationToken, Task> previous, IAsyncPolicy policy, IMutable<bool> scope)
	{
		_previous = previous;
		_policy   = policy;
		_scope    = scope;
	}

	public async ValueTask Get(CancellationToken parameter)
	{
		using (_scope.Assigned(true))
		{
			await _policy.ExecuteAsync(_previous, parameter, true).Off();
		}
	}
}

public class PolicyAware<TIn, TOut> : IStopAware<TIn, TOut>
{
	readonly IStopAware<TIn, TOut> _previous;
	readonly IAsyncPolicy<TOut>    _policy;
	readonly IMutable<bool>        _scope;

	protected PolicyAware(IStopAware<TIn, TOut> previous, IResult<IAsyncPolicy> policy)
		: this(previous, policy.Get()) {}

	protected PolicyAware(IStopAware<TIn, TOut> previous, IAsyncPolicy policy)
		: this(previous, policy.AsAsyncPolicy<TOut>()) {}

	protected PolicyAware(IStopAware<TIn, TOut> previous, IResult<IAsyncPolicy<TOut>> policy)
		: this(previous, policy.Get()) {}

	protected PolicyAware(IStopAware<TIn, TOut> previous, IAsyncPolicy<TOut> policy)
		: this(previous, policy, CurrentPolicy.Default) {}

	protected PolicyAware(IStopAware<TIn, TOut> previous, IAsyncPolicy<TOut> policy, IMutable<bool> scope)
	{
		_previous = previous;
		_policy   = policy;
		_scope    = scope;
	}

	public async ValueTask<TOut> Get(Stop<TIn> parameter)
	{
		var (subject, stop) = parameter;
		using (_scope.Assigned(true))
		{
			return await _policy.ExecuteAsync(x => _previous.Allocate(new(subject, x)), stop, true).Off();
		}
	}
}