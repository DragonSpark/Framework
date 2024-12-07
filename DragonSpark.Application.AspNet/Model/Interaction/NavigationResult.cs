using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Components;

namespace DragonSpark.Application.Model.Interaction;

public class NavigationResult : InteractionResult, ICommand<NavigationManager>
{
	readonly string _path;
	readonly bool   _force;

	public NavigationResult(string path, bool force = false)
	{
		_path  = path;
		_force = force;
	}

	public void Execute(NavigationManager parameter)
	{
		parameter.NavigateTo(_path, _force);
	}
}