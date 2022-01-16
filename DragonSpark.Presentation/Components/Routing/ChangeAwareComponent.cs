﻿using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Routing;

/// <summary>
/// Attribution:
/// https://github.com/ShaunCurtis/CEC.Routing/blob/master/CEC.RoutingSample/Components/EditorComponentBase.cs
/// </summary>
public abstract class ChangeAwareComponent : ComponentBase, IRoutingComponent, IAsyncDisposable
{
	readonly Func<Task> _cancel;

	protected ChangeAwareComponent() => _cancel = OnNavigationCanceled;

	[Inject]
	public NavigationManager Navigation { get; set; } = default!;

	[Inject]
	public RouterSession Session { get; set; } = default!;

	[Parameter]
	public EventCallback Exited { get; set; }

	public string? PageUrl { get; set; }

	public abstract bool HasChanges { get; }

	protected override Task OnInitializedAsync()
	{
		PageUrl                    =  Navigation.Uri;
		Session.NavigationCanceled += OnNavigationCanceled;
		return base.OnInitializedAsync();
	}

	protected override Task OnAfterRenderAsync(bool firstRender)
		=> firstRender ? Session.Register(this).AsTask() : base.OnAfterRenderAsync(firstRender);

	void OnNavigationCanceled(object? sender, EventArgs e)
	{
		InvokeAsync(_cancel);
	}

	protected virtual Task OnNavigationCanceled()
	{
		StateHasChanged();
		return Task.CompletedTask;
	}

	protected virtual async Task Exit()
	{
		await Session.Unregister(this);
		var destination = Session.NavigationCancelledUrl;
		if (destination != null)
		{
			Navigation.NavigateTo(destination);
		}

		await Exited.InvokeAsync(this);
	}

	~ChangeAwareComponent()
	{
		OnDispose(false);
	}

	public async ValueTask DisposeAsync()
	{
		await OnDisposing();
		GC.SuppressFinalize(this);
	}

	protected virtual void OnDispose(bool disposing)
	{
		Session.NavigationCanceled -= OnNavigationCanceled;
	}

	protected virtual async ValueTask OnDisposing()
	{
		if (Session.ActiveComponent == this)
		{
			await Session.Unregister(this).ConfigureAwait(false);
		}
		OnDispose(true);
	}
}