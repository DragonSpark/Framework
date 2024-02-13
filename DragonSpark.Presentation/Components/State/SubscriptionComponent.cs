using DragonSpark.Application.Connections.Events;
using DragonSpark.Compose;
using DragonSpark.Model;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.State;

public abstract class SubscriptionComponent<T> : ComponentBase, IAsyncDisposable where T : notnull
{
	ISubscription? _connection;

	[Parameter]
	public EventCallback<T> Received { get; set; }

	protected override Task OnInitializedAsync()
	{
		_connection = DetermineSubscription();
		return _connection.Allocate();
	}

	protected abstract ISubscription DetermineSubscription();

	protected virtual Task OnReceive(T parameter) => InvokeAsync(() => Received.InvokeAsync(parameter));

	public ValueTask DisposeAsync() => _connection?.DisposeAsync() ?? Task.CompletedTask.ToOperation();
}

public abstract class SubscriptionComponent : SubscriptionComponent<None>;