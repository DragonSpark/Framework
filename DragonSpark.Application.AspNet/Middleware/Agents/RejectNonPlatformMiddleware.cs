using Microsoft.AspNetCore.Http;
using System.Buffers;

namespace DragonSpark.Application.AspNet.Middleware.Agents;

public sealed class RejectNonPlatformMiddleware : MiddlewareBase
{
	public static RejectNonPlatformMiddleware Default { get; } = new();

	RejectNonPlatformMiddleware() : this(DefaultNonPlatformEndpoints.Default) {}

	public RejectNonPlatformMiddleware(params ReadOnlySpan<string> paths)
		: this(SearchValues.Create(paths, StringComparison.OrdinalIgnoreCase), GetMaxLength(paths), Respond.Default) {}

	readonly SearchValues<string> _segments;
	readonly byte                 _length;
	readonly IRespond             _respond;

	public RejectNonPlatformMiddleware(SearchValues<string> segments, byte length, IRespond respond)
	{
		_segments = segments;
		_length   = length;
		_respond  = respond;
	}

	public override Task Get(MiddlewareInput parameter)
	{
		var (context, next) = parameter;
		var path = context.Request.Path.Value.AsSpan();
		if (path.EndsWith(".php", StringComparison.OrdinalIgnoreCase))
		{
			return _respond.Get(new(context, StatusCodes.Status404NotFound));
		}

		var prefix = path.Length > _length ? path[.._length] : path;
		if (prefix.IndexOfAny(_segments) is 0)
		{
			return _respond.Get(new(context, StatusCodes.Status404NotFound));
		}

		var headers = context.Request.Headers;
		return headers.ContainsKey("Next-Action") || headers.ContainsKey("X-Nextjs-Request-Id")
			       ? _respond.Get(new(context, StatusCodes.Status400BadRequest))
			       : next(context);
	}

	static byte GetMaxLength(ReadOnlySpan<string> paths)
	{
		byte result = 0;
		foreach (var entry in paths)
		{
			if (entry is { Length: > 0 } && entry.Length > result)
			{
				result = (byte)entry.Length;
			}
		}

		return result;
	}
}