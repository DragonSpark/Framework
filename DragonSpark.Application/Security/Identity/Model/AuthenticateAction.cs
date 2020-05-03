using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Model
{
	sealed class AuthenticateAction : IAuthenticateAction
	{
		readonly IAuthentication _authentication;

		public AuthenticateAction(IAuthentication authentication) => _authentication = authentication;

		public async ValueTask<IActionResult?> Get(CallbackContext parameter)
		{
			var (login, origin) = parameter;

			var result = await _authentication.Get(login);
			return result.Succeeded
				       ? (IActionResult)new LocalRedirectResult(origin)
				       : result.IsLockedOut
					       ? new RedirectToPageResult("./Lockout")
					       : null;
		}
	}
}