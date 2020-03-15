using DragonSpark.Application.Security.Identity.Profile;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection.Alterations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace DragonSpark.Application.Security.Identity.Model
{
	interface IAppliedAuthentication : IAlteration<ExternalLoginInfo> {}

	sealed class AppliedAuthentication : IAppliedAuthentication
	{
		readonly IAppliedPrincipal _principal;

		public AppliedAuthentication(IAppliedPrincipal principal) => _principal = principal;

		public ExternalLoginInfo Get(ExternalLoginInfo parameter)
			=> new ExternalLoginInfo(_principal.Get(parameter), parameter.LoginProvider, parameter.ProviderKey,
			                         parameter.ProviderDisplayName);
	}

	interface IExternalSignin : IOperationResult<ExternalLoginInfo, SignInResult> {}

	sealed class ExternalSignin<T> : IExternalSignin where T : class
	{
		readonly SignInManager<T> _authentication;

		public ExternalSignin(SignInManager<T> authentication) => _authentication = authentication;

		public ValueTask<SignInResult> Get(ExternalLoginInfo parameter)
			=> _authentication.ExternalLoginSignInAsync(parameter.LoginProvider, parameter.ProviderKey, true, true)
			                  .ToOperation();
	}

	public interface IAuthentication : IOperationResult<ExternalLoginInfo, SignInResult> {}

	sealed class Authentication : IAuthentication
	{
		readonly IExternalSignin         _signin;
		readonly IUserSynchronization    _synchronization;
		readonly ILogger<Authentication> _log;

		public Authentication(IExternalSignin signin, IUserSynchronization synchronization,
		                      ILogger<Authentication> log)
		{
			_signin          = signin;
			_synchronization = synchronization;
			_log             = log;
		}

		public async ValueTask<SignInResult> Get(ExternalLoginInfo parameter)
		{
			var result = await _signin.Get(parameter);

			if (result.Succeeded)
			{
				_log.LogInformation("[{Id}] {Name} logged in with {LoginProvider} provider.",
				                    parameter.ProviderKey, parameter.Principal.Identity.Name, parameter.LoginProvider);

				await _synchronization.Get(parameter);
			}

			return result;
		}
	}

	sealed class AuthenticateAction : IAuthenticateAction
	{
		readonly IAuthentication _authentication;

		public AuthenticateAction(IAuthentication authentication) => _authentication = authentication;

		public async ValueTask<IActionResult> Get(CallbackContext parameter)
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