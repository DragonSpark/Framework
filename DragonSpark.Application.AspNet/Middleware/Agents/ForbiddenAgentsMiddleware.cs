using DragonSpark.Compose;
using DragonSpark.Model.Sequences;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Application.AspNet.Middleware.Agents;

public class ForbiddenAgentsMiddleware : MiddlewareBase
{
	readonly Array<string> _forbidden;

	protected ForbiddenAgentsMiddleware(params string[] forbidden) => _forbidden = forbidden;

	public override Task Get(MiddlewareInput parameter)
	{
		var (context, next) = parameter;

		if (context.Request.Headers.UserAgent.ToString().NullIfEmpty() is {} agent &&
		    _forbidden.Open().Contains(agent, StringComparer.OrdinalIgnoreCase))
		{
			context.Response.StatusCode = StatusCodes.Status403Forbidden;
			return context.Response.WriteAsync("Forbidden");
		}

		return next(context);
	}
}