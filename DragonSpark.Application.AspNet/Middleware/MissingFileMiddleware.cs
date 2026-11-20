using DragonSpark.Model.Selection.Conditions;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Application.AspNet.Middleware;

public sealed class MissingFileMiddleware : MiddlewareBase
{
	public static MissingFileMiddleware Default { get; } = new();

	MissingFileMiddleware() : this(IsMissingFileRequest.Default, Respond.Default) {}

	readonly ICondition<HttpRequest> _condition;
	readonly IRespond                _respond;

	public MissingFileMiddleware(ICondition<HttpRequest> condition, IRespond respond)
	{
		_condition = condition;
		_respond   = respond;
	}

	public override Task Get(MiddlewareInput parameter)
	{
		var (context, next) = parameter;
		return _condition.Get(context.Request)
			       ? _respond.Get(new(context, StatusCodes.Status404NotFound))
			       : next(context);
	}
}