using DragonSpark.Compose;
using DragonSpark.Model.Operations;

namespace DragonSpark.Application.Security.Identity.Authentication.Persist;

sealed class ClearAwarePersistSignInWithMetadata<T> : Appending<PersistMetadataInput<T>>, IPersistSignInWithMetadata<T> where T : IdentityUser
{
	public ClearAwarePersistSignInWithMetadata(ClearExistingClaims<T> first, IPersistSignInWithMetadata<T> second)
		: base(Start.A.Selection<PersistMetadataInput<T>>()
		            .By.Calling(x => new PersistInput<T>(x.User, x.Claims))
		            .Select(first)
		            .Out(),
		       second) {}
}