using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection.Conditions;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DragonSpark.Application.Components.Validation
{
	public sealed class ModelValidationContext : Collection<ValidationResultMessage>,
	                                             ICondition<object>,
	                                             IResult<string>,
	                                             ICommand<string>
	{
		readonly ICollection<string> _paths;
		readonly HashSet<object>     _visited;

		public ModelValidationContext() : this(new List<string>(), new HashSet<object>()) {}

		public ModelValidationContext(ICollection<string> paths, HashSet<object> visited)
		{
			_paths   = paths;
			_visited = visited;
		}

		public bool Get(object parameter) => _visited.Add(parameter);

		public string Get() => string.Join('.', _paths);

		public void Execute(string parameter)
		{
			_paths.Add(parameter);
		}
	}
}