using DragonSpark.Presentation.Environment.Browser;
using Microsoft.JSInterop;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content.Sequences.Virtualization;

sealed class ConnectionAwareReference : ConnectionAwareDisposing, IJSObjectReference
{
	readonly IJSObjectReference _previous;
	readonly IDisposable        _reference;

	public ConnectionAwareReference(IJSObjectReference previous, IDisposable reference) : base(previous)
	{
		_previous  = previous;
		_reference = reference;
	}

	public ValueTask<TValue> InvokeAsync<TValue>(string identifier, object?[]? args)
		=> _previous.InvokeAsync<TValue>(identifier, args);

	public ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken,
	                                             object?[]? args)
		=> _previous.InvokeAsync<TValue>(identifier, cancellationToken, args);

	public override ValueTask DisposeAsync()
	{
		_reference.Dispose();
		return base.DisposeAsync();
	}
}