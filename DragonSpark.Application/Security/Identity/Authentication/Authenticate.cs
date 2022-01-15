using DragonSpark.Application.Security.Identity.Claims.Compile;
using DragonSpark.Compose;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Authentication;

sealed class Authenticate<T> : IAuthenticate<T> where T : IdentityUser
{
	readonly PersistAuthentication<T> _persist;
	readonly IClaims                  _claims;

	public Authenticate(PersistAuthentication<T> persist, IClaims claims)
	{
		_persist = persist;
		_claims  = claims;
	}

	public ValueTask Get(Login<T> parameter)
	{
		var (information, user) = parameter;
		var claims = _claims.Get(new(information.Principal, information.LoginProvider, information.ProviderKey));
		return _persist.Get(new StoreAuthenticationInput<T>(user, claims.Open()));
	}
}