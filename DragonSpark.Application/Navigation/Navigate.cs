using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Components;

namespace DragonSpark.Application.Navigation;

public class Navigate : ICommand<string>
{
	readonly NavigationManager _navigation;
	readonly bool              _force;

	public Navigate(NavigationManager navigation, bool force = false)
	{
		_navigation = navigation;
		_force      = force;
	}

	public void Execute(string parameter)
	{
		_navigation.NavigateTo(parameter, _force);
	}
}