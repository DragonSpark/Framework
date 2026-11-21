using Microsoft.AspNetCore.Http;
using System.Buffers;

namespace DragonSpark.Application.AspNet.Middleware.Agents;

public sealed class RejectNonPlatformMiddleware : MiddlewareBase
{
	public static RejectNonPlatformMiddleware Default { get; } = new();

	RejectNonPlatformMiddleware() : this(DefaultNonPlatformEndpoints.Default) {}

	public RejectNonPlatformMiddleware(params ReadOnlySpan<string> paths)
		: this(SearchValues.Create(paths, StringComparison.OrdinalIgnoreCase), Respond.Default) {}

	readonly SearchValues<string> _segments;
	readonly IRespond             _respond;

	public RejectNonPlatformMiddleware(SearchValues<string> segments, IRespond respond)
	{
		_segments = segments;
		_respond  = respond;
	}

	public override Task Get(MiddlewareInput parameter)
	{
		var (context, next) = parameter;
		var path = context.Request.Path.Value.AsSpan();
		if (path.EndsWith(".php", StringComparison.OrdinalIgnoreCase) || path.ContainsAny(_segments))
		{
			return _respond.Get(new(context, StatusCodes.Status404NotFound));
		}

		var headers = context.Request.Headers;
		return headers.ContainsKey("Next-Action") || headers.ContainsKey("X-Nextjs-Request-Id")
			       ? _respond.Get(new(context, StatusCodes.Status400BadRequest))
			       : next(context);
	}
}