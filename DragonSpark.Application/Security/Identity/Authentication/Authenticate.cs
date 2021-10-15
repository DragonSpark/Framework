using DragonSpark.Application.Security.Identity.Claims.Compile;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Authentication;

sealed class Authenticate<T> : IAuthenticate<T> where T : IdentityUser
{
	readonly IAuthentications<T> _authentication;
	readonly IClaims             _claims;
	readonly bool                _persist;

	public Authenticate(IAuthentications<T> authentication, IClaims claims, bool persist = true)
	{
		_authentication = authentication;
		_claims         = claims;
		_persist        = persist;
	}

	public async ValueTask Get(Login<T> parameter)
	{
		var (information, user) = parameter;
		using var authentication = _authentication.Get();
		var       claims = _claims.Get(new(information.Principal, information.LoginProvider, information.ProviderKey));
		await authentication.Subject.SignInWithClaimsAsync(user, _persist, claims).ConfigureAwait(false);
	}
}