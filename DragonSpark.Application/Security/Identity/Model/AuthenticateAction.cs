using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace DragonSpark.Application.Security.Identity.Model
{
	interface IExternalSignin : IOperationResult<ExternalLoginInfo, SignInResult> {}

	sealed class ExternalSignin<T> : IExternalSignin where T : class
	{
		readonly SignInManager<T> _authentication;

		public ExternalSignin(SignInManager<T> authentication) => _authentication = authentication;

		public ValueTask<SignInResult> Get(ExternalLoginInfo parameter)
			=> _authentication.ExternalLoginSignInAsync(parameter.LoginProvider, parameter.ProviderKey, false, true)
			                  .ToOperation();
	}

	sealed class AuthenticateAction<T> : IAuthenticateAction where T : IdentityUser
	{
		readonly IExternalSignin                _signin;
		readonly IUserSynchronization           _synchronization;
		readonly ILogger<AuthenticateAction<T>> _log;

		public AuthenticateAction(IExternalSignin signin, IUserSynchronization synchronization,
		                          ILogger<AuthenticateAction<T>> log)
		{
			_signin          = signin;
			_synchronization = synchronization;
			_log             = log;
		}

		public async ValueTask<IActionResult> Get(CallbackContext parameter)
		{
			var (login, origin) = parameter;

			var result = await _signin.Get(login);
			if (result.Succeeded)
			{
				_log.LogInformation("[{Id}] {Name} logged in with {LoginProvider} provider.",
				                    login.ProviderKey, login.Principal.Identity.Name, login.LoginProvider);

				await _synchronization.Get(login);

				return new LocalRedirectResult(origin);
			}

			return result.IsLockedOut ? new RedirectToPageResult("./Lockout") : null;
		}
	}
}