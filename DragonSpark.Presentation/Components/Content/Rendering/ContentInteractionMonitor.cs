using DragonSpark.Compose;
using DragonSpark.Presentation.Connections.Circuits;
using DragonSpark.Runtime.Execution;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content.Rendering;

partial class ContentInteractionMonitor
{
	readonly First                    _active  = new();
	Func<Task>                             _execute = default!;
	EventHandler<LocationChangedEventArgs> _changed = default!;

	[Inject]
	Rendered Rendered { get; set; } = default!;

	protected override void OnInitialized()
	{
		base.OnInitialized();
		_execute                   =  OnReady;
		_changed                   =  NavigationOnLocationChanged;
		Navigation.LocationChanged += _changed;
	}

	Task OnReady()
	{
		if (_active.Get())
		{
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
		else if (!Rendered.Get())
		{
			Rendered.Execute(true);
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