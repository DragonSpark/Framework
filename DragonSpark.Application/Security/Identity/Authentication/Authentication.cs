using DragonSpark.Application.Security.Identity.Profile;
using DragonSpark.Compose;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Authentication;

sealed class Authentication : IAuthentication
{
	readonly IExternalSignin         _signin;
	readonly IUserSynchronization    _synchronization;
	readonly ILogger<Authentication> _log;

	public Authentication(IExternalSignin signin, IUserSynchronization synchronization, ILogger<Authentication> log)
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
			_log.LogInformation("[{Id}] {LoginProvider} user with key {Key} logged in.",
			                    parameter.Principal.UserName(), parameter.LoginProvider, parameter.ProviderKey);

			await _synchronization.Await(parameter);
		}

		return result;
	}
}