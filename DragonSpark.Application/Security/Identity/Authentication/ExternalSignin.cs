using DragonSpark.Application.Security.Identity.Profile;
using DragonSpark.Compose;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Authentication;

sealed class ExternalSignin<T> : IExternalSignin where T : IdentityUser
{
	readonly ILocateUser<T>          _locate;
	readonly AuthenticateUser<T>     _authenticate;
	readonly AuthenticateExternal _external;

	public ExternalSignin(ILocateUser<T> locate, AuthenticateUser<T> authenticate, AuthenticateExternal external)
	{
		_locate       = locate;
		_authenticate = authenticate;
		_external     = external;
	}

	public async ValueTask<SignInResult> Get(ExternalLoginInfo parameter)
	{
		var user = await _locate.Await(parameter);
		if (user is not null)
		{
			await _authenticate.Await(new(parameter, user));
			return SignInResult.Success;
		}
		await _external.Await(parameter);
		return SignInResult.Failed;
	}
}