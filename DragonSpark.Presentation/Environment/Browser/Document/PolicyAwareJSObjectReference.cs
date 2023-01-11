using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.JSInterop;
using Polly;
using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Environment.Browser.Document;

sealed class PolicyAwareJSObjectReference : IJSObjectReference
{
	readonly IJSObjectReference _previous;
	readonly IAsyncPolicy       _policy;
	readonly IOperation         _dispose;

	public PolicyAwareJSObjectReference(IJSObjectReference previous)
		: this(previous, DurableEvaluatePolicy.Default.Get()) {}

	public PolicyAwareJSObjectReference(IJSObjectReference previous, IAsyncPolicy policy)
		: this(previous, policy, new DisposeReference(previous, policy)) {}

	public PolicyAwareJSObjectReference(IJSObjectReference previous, IAsyncPolicy policy, IOperation dispose)
	{
		_previous = previous;
		_policy   = policy;
		_dispose  = dispose;
	}

	public ValueTask<TValue> InvokeAsync<TValue>(string identifier, object?[]? args)
		=> _policy.AsAsyncPolicy<TValue>()
		          .ExecuteAsync(() => _previous.InvokeAsync<TValue>(identifier, args).AsTask())
		          .ToOperation();

	public ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken,
	                                             object?[]? args)
		=> _policy.AsAsyncPolicy<TValue>()
		          .ExecuteAsync(() => _previous.InvokeAsync<TValue>(identifier, cancellationToken, args).AsTask())
		          .ToOperation();

	public ValueTask DisposeAsync() => _dispose.Get();
}