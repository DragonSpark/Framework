using System.Threading.Tasks;
using DragonSpark.Application.AspNet.Security.Identity.Profile;
using DragonSpark.Compose;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication;

sealed class Authentication(IExternalSignin signin, IUserSynchronization synchronization, ILogger<Authentication> log)
	: IAuthentication
{
	public async ValueTask<SignInResult> Get(ExternalLoginInfo parameter)
	{
		var result = await signin.Get(parameter).On();

		if (result.Succeeded)
		{
			log.LogInformation("[{Id}] {LoginProvider} user with key {Key} logged in",
			                    parameter.Principal.Number(), parameter.LoginProvider, parameter.ProviderKey);

			await synchronization.Off(parameter);
		}

		return result;
	}
}
