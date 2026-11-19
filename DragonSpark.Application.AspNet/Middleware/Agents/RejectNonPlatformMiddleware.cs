using Microsoft.AspNetCore.Http;

namespace DragonSpark.Application.AspNet.Middleware.Agents;

public sealed class RejectNonPlatformMiddleware : MiddlewareBase
{
	public static RejectNonPlatformMiddleware Default { get; } = new();

	RejectNonPlatformMiddleware() {}

	public override Task Get(MiddlewareInput parameter)
	{
		var (context, next) = parameter;
		var path         = context.Request.Path.Value ?? string.Empty;
		if (path.EndsWith(".php", StringComparison.OrdinalIgnoreCase) ||
		    path.Contains("/wp-", StringComparison.OrdinalIgnoreCase) ||
		    path.Contains("/.env", StringComparison.OrdinalIgnoreCase) ||
		    path.Contains("/.git", StringComparison.OrdinalIgnoreCase) ||
		    path.Contains("/xmlrpc", StringComparison.OrdinalIgnoreCase) ||
		    path.Contains("/phpmyadmin", StringComparison.OrdinalIgnoreCase))
		{
			context.Response.StatusCode = StatusCodes.Status404NotFound;
			return Task.CompletedTask;
		}

		if (context.Request.Headers.ContainsKey("Next-Action") ||
		    context.Request.Headers.ContainsKey("X-Nextjs-Request-Id"))
		{
			context.Response.StatusCode = StatusCodes.Status400BadRequest;
			return Task.CompletedTask;
		}

		return next(context);
	}
}