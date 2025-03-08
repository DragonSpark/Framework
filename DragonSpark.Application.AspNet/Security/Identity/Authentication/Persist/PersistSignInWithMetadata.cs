using DragonSpark.Compose;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication.Persist;

sealed class PersistSignInWithMetadata<T> : IPersistSignInWithMetadata<T> where T : IdentityUser
{
	readonly IAuthentications<T> _authentications;

	public PersistSignInWithMetadata(IAuthentications<T> authentications) => _authentications = authentications;

	public async ValueTask Get(PersistMetadataInput<T> parameter)
	{
		var (user, metadata, claims) = parameter;
		using var authentication = _authentications.Get();
		using var open           = claims;
		await authentication.Subject.SignInWithClaimsAsync(user, metadata, open).Off();
	}
}