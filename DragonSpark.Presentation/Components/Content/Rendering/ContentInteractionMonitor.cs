using DragonSpark.Compose;
using DragonSpark.Runtime.Execution;
using Microsoft.AspNetCore.Components.Routing;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content.Rendering;

partial class ContentInteractionMonitor
{
	readonly First                         _active  = new(), _rendered = new();
	Func<Task>                             _execute = default!;
	EventHandler<LocationChangedEventArgs> _changed = default!;

	protected override void OnInitialized()
	{
		base.OnInitialized();
		_execute                   =  OnReady;
		_changed                   =  NavigationOnLocationChanged;
		Navigation.LocationChanged += _changed;
		Monitor.Execute();
	}

	Task OnReady()
	{
		if (_active.Get())
		{
			Monitor.Execute();
			Interaction.Execute();
		}

		return Task.CompletedTask;
	}

	void NavigationOnLocationChanged(object? sender, LocationChangedEventArgs e)
	{
		Navigation.LocationChanged -= _changed;
		Interaction.Execute();
	}

	protected override void OnAfterRender(bool firstRender)
	{
		if (firstRender)
		{
			StateHasChanged();
		}
		else if (_rendered.Get())
		{
			Debounce(_execute, (int)PreRenderingWindow.Default.Get().TotalMilliseconds);
		}
	}

	public override void Dispose()
	{
		_active.Get();
		base.Dispose();
		/*if (Rendered.Get())
		{
			Interaction.Execute();
		}*/
		Navigation.LocationChanged -= _changed;
	}
}