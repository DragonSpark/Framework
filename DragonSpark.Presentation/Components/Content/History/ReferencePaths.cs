using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection.Stores;
using DragonSpark.Text;
using Microsoft.AspNetCore.Components;

namespace DragonSpark.Presentation.Components.Content.History;

sealed class ReferencePaths : IFormatter<object>, ICommand
{
	readonly NavigationManager                                 _navigation;
	readonly ITable<string, StandardTable<Type, List<object>>> _references;

	public ReferencePaths(NavigationManager navigation, References references)
	{
		_navigation = navigation;
		_references = references;
	}

	public string Get(object parameter)
	{
		var type       = parameter.GetType();
		var references = _references.Get(_navigation.Uri).Get(type);
		if (!references.Contains(parameter))
		{
			references.Add(parameter);
		}

		return $"{type.FullName}+{references.IndexOf(parameter)}";
	}

	public void Execute(None parameter)
	{
		_references.Get(_navigation.Uri).Execute();
	}
}