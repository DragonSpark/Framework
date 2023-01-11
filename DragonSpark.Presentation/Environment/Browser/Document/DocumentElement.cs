using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.JSInterop;
using Polly;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Environment.Browser.Document;

sealed class DocumentElement : IAsyncDisposable
{
	readonly IJSObjectReference _instance;
	readonly IOperation         _dispose;

	public DocumentElement(IJSObjectReference instance) : this(instance, new DisposeReference(instance)) {}

	public DocumentElement(IJSObjectReference instance, IOperation dispose)
	{
		_instance = instance;
		_dispose  = dispose;
	}

	public ValueTask AddClass(string name) => _instance.InvokeVoidAsync(nameof(AddClass), name);

	public ValueTask RemoveClass(string name) => _instance.InvokeVoidAsync(nameof(RemoveClass), name);

	public ValueTask DisposeAsync() => _dispose.Get();
}

// TODO

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
		_previous     = previous;
		_policy       = policy;
		_dispose = dispose;
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