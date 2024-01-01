using DragonSpark.Application.Connections.Client;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Selection.Conditions;
using DragonSpark.Presentation.Components.Content.Rendering;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.State;

public abstract class ReceiveParameterViewComponent<T> : ComponentBase, IAsyncDisposable where T : notnull
{
	ISubscription? _connection;

	[Inject]
	RenderStateStore State { get; set; } = default!;

	[Parameter]
	public EventCallback<T> Received { get; set; }

	protected override Task OnInitializedAsync()
	{
		switch (State.Get())
		{
			case RenderState.Ready:
			case RenderState.Established:
				_connection = DetermineSubscription();
				return _connection.Allocate();
		}

		return base.OnInitializedAsync();
	}

	protected abstract ISubscription DetermineSubscription();

	protected virtual Task OnReceive(T parameter)
	{
		return Received.InvokeAsync(parameter);
	}

	public ValueTask DisposeAsync() => _connection?.DisposeAsync() ?? Task.CompletedTask.ToOperation();
}

// TODO

public abstract class FilteredReceiveParameterViewComponent<T> : ReceiveParameterViewComponent<T> where T : notnull
{
	readonly static IDepending<T> DefaultCondition = Is.Always<T>().Operation().Out();

	[Parameter]
	public IDepending<T> Condition { get; set; } = DefaultCondition;

	protected override async Task OnReceive(T parameter)
	{
		if (await Condition.Get(parameter))
		{
			await base.OnReceive(parameter).ConfigureAwait(false);
		}
	}
}

public sealed class FilteredSubscriberComponent<T> : FilteredReceiveParameterViewComponent<T> where T : notnull
{
	[Parameter]
	public uint? Recipient { get; set; }

	[Parameter]
	public ISubscriber<T> Registration { get; set; } = default!;

	protected override ISubscription DetermineSubscription() => Registration.Get(new(Recipient, OnReceive));
}

public sealed class SubscriberComponent<T> : ReceiveParameterViewComponent<T> where T : notnull
{
	[Parameter]
	public uint? Recipient { get; set; }

	[Parameter]
	public ISubscriber<T> Registration { get; set; } = default!;

	protected override ISubscription DetermineSubscription() => Registration.Get(new(Recipient, OnReceive));
}