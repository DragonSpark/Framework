using DragonSpark.Compose;
using DragonSpark.Runtime.Execution;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content.Rendering;

partial class RenderStateMonitorComponent
{
	readonly First                         _active  = new(), _rendered = new();
	Func<Task>                             _ready   = default!;
	EventHandler<LocationChangedEventArgs> _changed = default!;
	PersistingComponentStateSubscription   _subscription;

	protected override void OnInitialized()
	{
		base.OnInitialized();

		_changed                   =  NavigationOnLocationChanged;
		Navigation.LocationChanged += _changed;

		_subscription = State.RegisterOnPersisting(OnPersist);

		if (State.TryTakeFromJson<RenderState>(Key, out var restored))
		{
			Monitor.Execute(restored);
		}
	}

	string Key { get; set; } = A.Type<RenderStateMonitorComponent>().FullName.Verify();

	Task OnPersist()
	{
		State.PersistAsJson(Key, RenderState.Ready);
		return Task.CompletedTask;
	}

	Task OnReady()
	{
		if (_active.Get())
		{
			Monitor.Execute();
		}

		return Task.CompletedTask;
	}

	void NavigationOnLocationChanged(object? sender, LocationChangedEventArgs e)
	{
		Navigation.LocationChanged -= _changed;
		Monitor.Execute();
	}

	protected override void OnAfterRender(bool firstRender)
	{
		if (firstRender)
		{
			StateHasChanged();
		}
		else if (_rendered.Get())
		{
			Debounce(_ready, (int)PreRenderingWindow.Default.Get().TotalMilliseconds);
		}
	}

	public override void Dispose()
	{
		_active.Get();
		_subscription.Dispose();
		base.Dispose();
		Navigation.LocationChanged -= _changed;
	}
}