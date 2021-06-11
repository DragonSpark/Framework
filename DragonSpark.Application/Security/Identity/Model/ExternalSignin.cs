using DragonSpark.Compose;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Model
{
	sealed class ExternalSignin<T> : IExternalSignin where T : IdentityUser
	{
		readonly ILocateUser<T>   _locate;
		readonly IAuthenticate<T> _authenticate;

		public ExternalSignin(ILocateUser<T> locate, IAuthenticate<T> authenticate)
		{
			_locate       = locate;
			_authenticate = authenticate;
		}

		public async ValueTask<SignInResult> Get(ExternalLoginInfo parameter)
		{
			var user = await _locate.Await(parameter);
			if (user != null)
			{
				await _authenticate.Await(new(parameter, user));
				return SignInResult.Success;
			}

			return SignInResult.Failed;
		}
	}
}