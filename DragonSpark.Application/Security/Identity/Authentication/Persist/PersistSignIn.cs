using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Authentication.Persist;

sealed class PersistSignIn<T> : IPersistSignIn<T> where T : IdentityUser
{
	readonly IAuthentications<T> _authentication;
	readonly bool                _persist;

	public PersistSignIn(IAuthentications<T> authentication, bool persist = true)
	{
		_authentication = authentication;
		_persist        = persist;
	}

	public async ValueTask Get(PersistInput<T> parameter)
	{
		var (user, claims) = parameter;

		using var authentication = _authentication.Get();
		await authentication.Subject.SignInWithClaimsAsync(user, _persist, claims.Open()).ConfigureAwait(false);
	}
}