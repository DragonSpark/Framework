using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Runtime;
using Microsoft.JSInterop;
using Polly;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Environment.Browser.Document;

public class PolicyAwareJSObjectReference : Disposing, IJSObjectReference
{
	readonly IJSObjectReference _previous;
	readonly object             _policy;

	public PolicyAwareJSObjectReference(IJSObjectReference previous)
		: this(previous, DurableEvaluatePolicy.Default.Get()) {}

	public PolicyAwareJSObjectReference(IJSObjectReference previous, object policy)
		: this(previous, policy, new DisposeReference(previous)) {}

	public PolicyAwareJSObjectReference(IJSObjectReference previous, object policy, IOperation dispose)
		: base(dispose.Get)
	{
		_previous = previous;
		_policy   = policy;
	}

	public ValueTask<TValue> InvokeAsync<TValue>(string identifier, object?[]? args)
	{
		var policy = _policy as IAsyncPolicy<TValue> ??
		             (_policy is IAsyncPolicy p ? p.AsAsyncPolicy<TValue>() : throw new InvalidOperationException());
		return policy.ExecuteAsync(() => _previous.InvokeAsync<TValue>(identifier, args).AsTask())
		             .ToOperation();
	}

	public ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken,
	                                             object?[]? args)
	{
		var policy = _policy as IAsyncPolicy<TValue> ??
		             (_policy is IAsyncPolicy p ? p.AsAsyncPolicy<TValue>() : throw new InvalidOperationException());
		return policy.ExecuteAsync(() => _previous.InvokeAsync<TValue>(identifier, cancellationToken, args).AsTask())
		             .ToOperation();
	}
}