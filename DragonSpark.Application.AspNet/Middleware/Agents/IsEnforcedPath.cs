using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Model.Sequences;
using System.Buffers;

namespace DragonSpark.Application.AspNet.Middleware.Agents;

sealed class IsEnforcedPath : ICondition<string>
{
	readonly SearchValues<string> _paths;

	public IsEnforcedPath(Array<string> paths,
	                      StringComparison comparison = StringComparison.OrdinalIgnoreCase)
		: this(SearchValues.Create(paths.Open(), comparison)) {}

	public IsEnforcedPath(SearchValues<string> paths) => _paths = paths;

	public bool Get(string parameter) => parameter.AsSpan().IndexOfAny(_paths) is 0;
}