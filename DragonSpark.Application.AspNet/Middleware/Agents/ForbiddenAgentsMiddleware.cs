using Microsoft.AspNetCore.Http;
using System.Buffers;

namespace DragonSpark.Application.AspNet.Middleware.Agents;

public class ForbiddenAgentsMiddleware : MiddlewareBase
{
	readonly SearchValues<string> _search;
	readonly IRespond             _respond;

	protected ForbiddenAgentsMiddleware(params string[] forbidden)
		: this(SearchValues.Create(forbidden, StringComparison.OrdinalIgnoreCase)) {}

	protected ForbiddenAgentsMiddleware(SearchValues<string> search) : this(search, Respond.Default) {}

	protected ForbiddenAgentsMiddleware(SearchValues<string> search, IRespond respond)
	{
		_search  = search;
		_respond = respond;
	}

	public override Task Get(MiddlewareInput parameter)
	{
		var (context, next) = parameter;
		foreach (var value in context.Request.Headers.UserAgent)
		{
			if (value.AsSpan().ContainsAny(_search))
			{
				return _respond.Get(new(context, StatusCodes.Status403Forbidden));
			}
		}

		return next(context);
	}
}