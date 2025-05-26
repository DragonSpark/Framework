using DragonSpark.Application.AspNet.Security.Identity.Authentication;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Security.Identity.Model;

sealed class AuthenticateRequest : IAuthenticateRequest
{
	readonly IAuthentication _authentication;

	public AuthenticateRequest(IAuthentication authentication) => _authentication = authentication;

	public async ValueTask<IActionResult?> Get(Stop<Challenged> parameter)
	{
		var ((login, origin), stop) = parameter;

		var authentication = await _authentication.Off(new(login, stop));
		var result = authentication.IsLockedOut
			             ? new RedirectToPageResult("./Lockout")
			             : authentication.Succeeded
				             ? new LocalRedirectResult(origin)
				             : default(IActionResult?);
		return result;
	}
}