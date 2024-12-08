using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Components;

namespace DragonSpark.Application.AspNet.Navigation;

public class Navigate : ICommand<string>
{
	readonly NavigationManager _navigation;
	readonly bool              _force;
	readonly bool              _replace;

	public Navigate(NavigationManager navigation, bool force = false, bool replace = false)
	{
		_navigation   = navigation;
		_force        = force;
		_replace = replace;
	}

	public void Execute(string parameter)
	{
		_navigation.NavigateTo(parameter, _force, _replace);
	}
}