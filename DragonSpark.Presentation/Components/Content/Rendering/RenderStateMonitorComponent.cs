using DragonSpark.Compose;
using DragonSpark.Runtime.Execution;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using System;

namespace DragonSpark.Presentation.Components.Content.Rendering;

public sealed class RenderStateMonitorComponent : Microsoft.AspNetCore.Components.ComponentBase, IDisposable
{
	readonly First                         _rendered = new();
	EventHandler<LocationChangedEventArgs> _changed  = default!;

	[Inject]
	NavigationManager Navigation { get; set; } = default!;

	[Inject]
	RenderStateMonitor Monitor { get; set; } = default!;

	[Inject]
	PersistentComponentState State { get; set; } = default!;

	protected override void OnInitialized()
	{
		base.OnInitialized();

		_changed                   =  NavigationOnLocationChanged;
		Navigation.LocationChanged += _changed;
	}

	string Key { get; set; } = A.Type<RenderStateMonitorComponent>().FullName.Verify();

	void NavigationOnLocationChanged(object? sender, LocationChangedEventArgs e)
	{
		// Navigation.LocationChanged -= _changed;
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
			Monitor.Execute();
		}
	}

	public void Dispose()
	{
		Navigation.LocationChanged -= _changed;
	}
}