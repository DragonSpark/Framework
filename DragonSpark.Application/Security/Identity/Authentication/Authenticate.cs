using DragonSpark.Application.Security.Identity.Authentication.Persist;
using DragonSpark.Application.Security.Identity.Claims.Compile;
using DragonSpark.Compose;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Authentication;

sealed class Authenticate<T> : IAuthenticate<T> where T : IdentityUser
{
	readonly IPersistSignIn<T> _persist;
	readonly IClaims     _claims;

	public Authenticate(IPersistSignIn<T> persist, IClaims claims)
	{
		_persist = persist;
		_claims  = claims;
	}

	public ValueTask Get(Login<T> parameter)
	{
		var (information, user) = parameter;
		var claims = _claims.Get(new(information.Principal, information.LoginProvider, information.ProviderKey));
		return _persist.Get(new(user, claims.Open()));
	}
}