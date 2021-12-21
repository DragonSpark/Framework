using DragonSpark.Application.Security.Identity.Authentication;
using DragonSpark.Compose;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Model;

[UsedImplicitly]
sealed class AuthenticateRequest : IAuthenticateRequest
{
	readonly IAuthentication _authentication;

	public AuthenticateRequest(IAuthentication authentication) => _authentication = authentication;

	public async ValueTask<IActionResult?> Get(Challenged parameter)
	{
		var (login, origin) = parameter;

		var authentication = await _authentication.Await(login);
		var result = authentication.IsLockedOut
			             ? new RedirectToPageResult("./Lockout")
			             : authentication.Succeeded
				             ? new LocalRedirectResult(origin)
				             : default(IActionResult?);
		return result;
	}
}