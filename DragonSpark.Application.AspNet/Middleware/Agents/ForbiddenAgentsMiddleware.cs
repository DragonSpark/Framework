using DragonSpark.Compose;
using DragonSpark.Model.Sequences;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Application.AspNet.Middleware.Agents;

public class ForbiddenAgentsMiddleware : MiddlewareBase
{
	readonly Array<string> _forbidden;

	protected ForbiddenAgentsMiddleware(params string[] forbidden) => _forbidden = forbidden;

	public override async Task Get(MiddlewareInput parameter)
	{
		var (context, next) = parameter;

		if (context.Request.Headers.UserAgent.ToString().NullIfEmpty() is {} agent &&
		    _forbidden.Open().Contains(agent, StringComparer.OrdinalIgnoreCase))
		{
			context.Response.StatusCode = StatusCodes.Status403Forbidden;
			await context.Response.WriteAsync("Forbidden").Off();
			return;
		}

		await next(context).Off();
	}
}