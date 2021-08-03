using DragonSpark.Application.Security.Identity.Profile;
using DragonSpark.Compose;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Authentication
{
	sealed class ExternalSignin<T> : IExternalSignin where T : IdentityUser
	{
		readonly ILocateUser<T>             _locate;
		readonly IAuthenticate<T>           _authenticate;
		readonly ClearCurrentAuthentication _clear;

		public ExternalSignin(ILocateUser<T> locate, IAuthenticate<T> authenticate, ClearCurrentAuthentication clear)
		{
			_locate       = locate;
			_authenticate = authenticate;
			_clear        = clear;
		}

		public async ValueTask<SignInResult> Get(ExternalLoginInfo parameter)
		{
			var user = await _locate.Await(parameter);
			if (user != null)
			{
				await _authenticate.Await(new(parameter, user));
				_clear.Execute(parameter.Principal);
				return SignInResult.Success;
			}

			return SignInResult.Failed;
		}
	}
}