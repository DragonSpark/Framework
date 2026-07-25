using System.Collections.ObjectModel;
using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Application.AspNet.Components.Validation;

public sealed class GraphValidationContext : Collection<ValidationResultMessage>,
											 ICondition<object>,
											 IResult<string>,
											 ICommand<string>
{
	readonly Stack<string>   _paths;
	readonly HashSet<object> _visited;

	public GraphValidationContext() : this([], []) {}

	public GraphValidationContext(Stack<string> paths, HashSet<object> visited)
	{
		_paths   = paths;
		_visited = visited;
	}

	public bool Get(object parameter) => _visited.Add(parameter);

	public string Get() => _paths.Only() ?? string.Empty;

	public void Execute(string parameter)
	{
		switch (_paths.Count)
		{
			case 0:
				_paths.Push(parameter);
				break;
		}
	}
}