using DragonSpark.Application.AspNet.Security.Identity.Authentication;
using DragonSpark.Model.Selection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace DragonSpark.Application.AspNet.Security.Identity;

sealed class ComposeContextSession<T> : ISelect<HttpContext, SignInManager<T>> where T : class
{
	readonly IAuthentications<T> _sessions;

	public ComposeContextSession(IAuthentications<T> sessions) => _sessions = sessions;

	public SignInManager<T> Get(HttpContext parameter)
	{
		var result = _sessions.Get();
		parameter.Response.RegisterForDispose(result);
		return result.Subject;
	}
}