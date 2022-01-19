using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Authentication.Persist;

sealed class PersistSignInWithMetadata<T> : IPersistSignInWithMetadata<T> where T : IdentityUser
{
	readonly IAuthentications<T> _authentications;

	public PersistSignInWithMetadata(IAuthentications<T> authentications) => _authentications = authentications;

	public async ValueTask Get(PersistMetadataInput<T> parameter)
	{
		var (user, metadata, claims) = parameter;
		using var authentication = _authentications.Get();
		var       open           = claims.Open();
		await authentication.Subject.SignInWithClaimsAsync(user, metadata, open).ConfigureAwait(false);
	}
}