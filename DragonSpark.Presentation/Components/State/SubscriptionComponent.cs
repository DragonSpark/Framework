using DragonSpark.Application.Connections.Events;
using DragonSpark.Application.Runtime;
using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.State;

public abstract class SubscriptionComponent<T> : ComponentBase, IAsyncDisposable where T : notnull
{
	ISubscription? _connection;
	IOperation<T>  _operation = null!;

	[Parameter]
	public EventCallback<T> Received { get; set; }

	[Parameter]
	public TimeSpan Throttle { get; set; } = TimeSpan.FromMilliseconds(250);

	protected override Task OnInitializedAsync()
	{
		_operation  = new ThrottleOperation<T>(Fire, Throttle);
		_connection = DetermineSubscription();
		return _connection.Allocate();
	}

	protected abstract ISubscription DetermineSubscription();

	protected virtual Task OnReceive(T parameter) => _operation.Get(parameter).AsTask();

	Task Fire(T parameter) => InvokeAsync(() => Received.Invoke(parameter));

	public ValueTask DisposeAsync() => _connection?.DisposeAsync() ?? Task.CompletedTask.ToOperation();
}

public abstract class SubscriptionComponent : SubscriptionComponent<None>;