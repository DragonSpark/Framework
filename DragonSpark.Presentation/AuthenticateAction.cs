using DragonSpark.Compose;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace DragonSpark.Presentation
{
	sealed class AuthenticateAction<T> : IAuthenticateAction where T : IdentityUser
	{
		readonly SignInManager<T>               _authentication;
		readonly ILogger<AuthenticateAction<T>> _log;

		public AuthenticateAction(SignInManager<T> authentication, ILogger<AuthenticateAction<T>> log)
		{
			_authentication = authentication;
			_log            = log;
		}

		public async ValueTask<IActionResult> Get(CallbackContext parameter)
		{
			var (login, origin) = parameter;

			var result = await _authentication.ExternalLoginSignInAsync(login.LoginProvider,
			                                                            login.ProviderKey, false, true);
			if (result.Succeeded)
			{
				_log.LogInformation("[{Id}] {Name} logged in with {LoginProvider} provider.",
				                    login.ProviderKey, login.Principal.Identity.Name, login.LoginProvider);

				return new LocalRedirectResult(origin);
			}

			return result.IsLockedOut ? new RedirectToPageResult("./Lockout") : null;
		}
	}
}