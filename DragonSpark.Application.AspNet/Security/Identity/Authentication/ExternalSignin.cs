using DragonSpark.Application.AspNet.Security.Identity.Profile;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication;

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

	public async ValueTask<SignInResult> Get(Stop<ExternalLoginInfo> parameter)
	{
		var user = await _locate.Off(parameter);
		if (user is not null)
		{
			await _authenticate.Off(new(parameter, user));
			return SignInResult.Success;
		}
		await _external.Off(parameter);
		return SignInResult.Failed;
	}
}