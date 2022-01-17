using DragonSpark.Compose;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Authentication.Persist;

sealed class PersistRefresh<T> : IPersistRefresh<T> where T : IdentityUser
{
	readonly IAuthentications<T> _authentications;
	readonly IPersistClaims<T>   _claims;

	public PersistRefresh(IAuthentications<T> authentications, IPersistClaims<T> claims)
	{
		_authentications = authentications;
		_claims          = claims;
	}

	public async ValueTask Get(PersistMetadataInput<T> parameter)
	{
		var (user, metadata, claims) = parameter;
		using var authentication = _authentications.Get();
		var       open           = claims.Open();
		await authentication.Subject.SignInWithClaimsAsync(user, metadata, open).ConfigureAwait(false);
		await _claims.Await(new Claims<T>(user, claims));
	}
}