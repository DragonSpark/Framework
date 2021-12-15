using DragonSpark.Compose;
using Microsoft.AspNetCore.Components.Routing;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content.Rendering;

partial class ContentInteractionMonitor
{
	Func<Task>                             _execute = default!;
	EventHandler<LocationChangedEventArgs> _changed = default!;

	bool Rendered { get; set; }

	protected override void OnInitialized()
	{
		base.OnInitialized();
		_execute                   =  Interaction.Then().Operation().Allocate().Get().Get;
		_changed                   =  NavigationOnLocationChanged;
		Navigation.LocationChanged += _changed;
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
		else
		{
			Debounce(_execute, (int)PreRenderingWindow.Default.Get().TotalMilliseconds);
			Rendered = true;
		}
	}

	public override void Dispose()
	{
		base.Dispose();
		if (Rendered)
		{
			Interaction.Execute();
		}
	}
}