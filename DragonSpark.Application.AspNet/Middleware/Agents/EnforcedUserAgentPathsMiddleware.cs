using DragonSpark.Model.Selection.Conditions;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Application.AspNet.Middleware.Agents;

public class EnforcedUserAgentPathsMiddleware : MiddlewareBase
{
	readonly string                  _key, _content;
	readonly ICondition<HttpRequest> _challenge;

	protected EnforcedUserAgentPathsMiddleware(params string[] paths)
		: this(ChallengeStateKey.Default, ChallengeContent.Default,
		       new ShouldChallenge(IsAllowed.Default, paths)) {}

	protected EnforcedUserAgentPathsMiddleware(string key, string content, ICondition<HttpRequest> challenge)
	{
		_key       = key;
		_content   = content;
		_challenge = challenge;
	}

	public override Task Get(MiddlewareInput parameter)
	{
		var (context, next) = parameter;
		if (_challenge.Get(context.Request))
		{
			var response = context.Response;
			response.StatusCode           = StatusCodes.Status200OK;
			response.ContentType          = "text/html; charset=utf-8";
			response.Headers.CacheControl = "no-store, no-cache, must-revalidate";
			response.Headers.Pragma       = "no-cache";

			response.Cookies.Append(_key, "1", new CookieOptions
			{
				Path     = "/",
				MaxAge   = TimeSpan.FromDays(1),
				SameSite = SameSiteMode.Lax,
				HttpOnly = true,
				Secure   = context.Request.IsHttps
			});

			return response.WriteAsync(_content);
		}

		return next(context);
	}
}