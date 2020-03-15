using DragonSpark.Compose;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Model {
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
}