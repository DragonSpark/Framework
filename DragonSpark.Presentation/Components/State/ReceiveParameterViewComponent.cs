﻿using DragonSpark.Application.Connections.Client;
using DragonSpark.Compose;
using DragonSpark.Model;
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

public abstract class ReceiveParameterViewComponent : ReceiveParameterViewComponent<None>;