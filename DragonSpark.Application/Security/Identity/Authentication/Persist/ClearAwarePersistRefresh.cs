using DragonSpark.Compose;
using DragonSpark.Model.Operations;

namespace DragonSpark.Application.Security.Identity.Authentication.Persist;

sealed class ClearAwarePersistRefresh<T> : Appending<PersistMetadataInput<T>>, IPersistRefresh<T> where T : IdentityUser
{
	public ClearAwarePersistRefresh(ClearExistingClaims<T> first, IPersistRefresh<T> second)
		: base(Start.A.Selection<PersistMetadataInput<T>>()
		            .By.Calling(x => new PersistInput<T>(x.User, x.Claims))
		            .Select(first)
		            .Out(),
		       second) {}
}