using DragonSpark.Compose;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Authentication.Persist;

sealed class PersistSignIn<T> : IPersistSignIn<T> where T : IdentityUser
{
	readonly IAuthentications<T> _authentication;
	readonly IPersistClaims<T>   _claims;
	readonly bool                _persist;

	public PersistSignIn(IAuthentications<T> authentication, IPersistClaims<T> claims, bool persist = true)
	{
		_authentication = authentication;
		_claims         = claims;
		_persist        = persist;
	}

	public async ValueTask Get(PersistInput<T> parameter)
	{
		var (user, claims) = parameter;

		using var authentication = _authentication.Get();
		await authentication.Subject.SignInWithClaimsAsync(user, _persist, claims.Open()).ConfigureAwait(false);
		await _claims.Await(new Claims<T>(user, claims));
	}
}