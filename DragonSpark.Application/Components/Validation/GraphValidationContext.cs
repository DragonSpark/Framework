using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection.Conditions;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DragonSpark.Application.Components.Validation
{
	public sealed class GraphValidationContext : Collection<ValidationResultMessage>,
	                                             ICondition<object>,
	                                             IResult<string>,
	                                             ICommand<string>
	{
		readonly Stack<string>   _paths;
		readonly HashSet<object> _visited;

		public GraphValidationContext() : this(new Stack<string>(), new HashSet<object>()) {}

		public GraphValidationContext(Stack<string> paths, HashSet<object> visited)
		{
			_paths   = paths;
			_visited = visited;
		}

		public bool Get(object parameter) => _visited.Add(parameter);

		public string Get() => _paths.Only().Account() ?? string.Empty;

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
}