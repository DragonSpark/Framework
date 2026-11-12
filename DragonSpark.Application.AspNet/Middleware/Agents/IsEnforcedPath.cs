using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Model.Sequences;

namespace DragonSpark.Application.AspNet.Middleware.Agents;

sealed class IsEnforcedPath : ICondition<string>
{
	readonly Array<string>    _paths;
	readonly StringComparison _comparison;

	public IsEnforcedPath(Array<string> paths,
	                      StringComparison comparison = StringComparison.InvariantCultureIgnoreCase)
	{
		_paths      = paths;
		_comparison = comparison;
	}

	public bool Get(string parameter)
	{
		for (var i = 0; i < _paths.Length; i++)
		{
			if (parameter.StartsWith(_paths[i], _comparison))
			{
				return true;
			}
		}

		return false;
	}
}