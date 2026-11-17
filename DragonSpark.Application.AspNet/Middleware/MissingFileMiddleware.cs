using DragonSpark.Model.Selection.Conditions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Application.AspNet.Middleware;

public sealed class MissingFileMiddleware : MiddlewareBase
{
	public static MissingFileMiddleware Default { get; } = new();

	MissingFileMiddleware() : this(IsMissingFileRequest.Default) {}

	readonly ICondition<HttpRequest> _condition;

	public MissingFileMiddleware(ICondition<HttpRequest> condition) => _condition = condition;

	public override Task Get(MiddlewareInput parameter)
	{
		var (context, next) = parameter;
		if (_condition.Get(context.Request))
		{
			if (context.Features.Get<IStatusCodePagesFeature>() is {} feature)
			{
				feature.Enabled = false;
			}

			context.Response.StatusCode = StatusCodes.Status404NotFound;
			return Task.CompletedTask;
		}

		return next(context);
	}
}