using DragonSpark.Compose;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Model.Sequences;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Application.AspNet.Middleware.Agents;

sealed class ShouldChallenge : ICondition<HttpRequest>
{
	readonly ICondition<HttpRequest> _allowed;
	readonly ICondition<string>      _path;

	public ShouldChallenge(Array<string> paths) : this(IsAllowed.Default, paths) {}

	public ShouldChallenge(ICondition<HttpRequest> allowed, Array<string> paths)
		: this(allowed, new IsEnforcedPath(paths)) {}

	public ShouldChallenge(ICondition<HttpRequest> allowed, ICondition<string> path)
	{
		_allowed = allowed;
		_path    = path;
	}

	public bool Get(HttpRequest parameter)
		=> (HttpMethods.IsGet(parameter.Method) || HttpMethods.IsHead(parameter.Method))
		   && parameter.Path.Value.NullIfEmpty() is {} current
		   && _path.Get(current)
		   && !_allowed.Get(parameter);
}