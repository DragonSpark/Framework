using DragonSpark.Application.AspNet.Security.Identity.Profile;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication;

sealed class Authentication(IExternalSignin signin, IUserSynchronization synchronization, ILogger<Authentication> log)
	: IAuthentication
{
	public async ValueTask<SignInResult> Get(Stop<ExternalLoginInfo> parameter)
	{
		var result = await signin.Get(parameter).On();
		if (result.Succeeded)
		{
			var (subject, stop) = parameter;
			log.LogInformation("[{Id}] {LoginProvider} user with key {Key} logged in",
			                   subject.Principal.Number(), subject.LoginProvider, subject.ProviderKey);

			await synchronization.Off(new(parameter, stop));
		}

		return result;
	}
}
