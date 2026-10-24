using DragonSpark.Compose;
using DragonSpark.Model.Operations.Selection;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Application.AspNet.Security.Identity;

sealed class ComposeContextApplicationUser<T> : ISelecting<HttpContext, T> where T : class
{
	readonly ContextSession<T> _session;

	public ComposeContextApplicationUser(ContextSession<T> session) => _session = session;

	public async ValueTask<T> Get(HttpContext parameter)
	{
		var result = await _session.Get(parameter).UserManager.GetUserAsync(parameter.User).Off();
		return result.Verify();
	}
}