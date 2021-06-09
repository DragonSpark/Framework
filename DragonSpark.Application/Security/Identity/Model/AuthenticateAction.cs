using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Model
{
	[UsedImplicitly]
	sealed class AuthenticateAction : IAuthenticateAction
	{
		readonly IAuthentication _authentication;

		public AuthenticateAction(IAuthentication authentication) => _authentication = authentication;

		public async ValueTask<IActionResult?> Get(Challenged parameter)
		{
			var (login, origin) = parameter;

			var authentication = await _authentication.Get(login);
			var result = authentication.Succeeded
				             ? new LocalRedirectResult(origin)
				             : authentication.IsLockedOut
					             ? new RedirectToPageResult("./Lockout")
					             : default(IActionResult?);
			return result;
		}
	}
}