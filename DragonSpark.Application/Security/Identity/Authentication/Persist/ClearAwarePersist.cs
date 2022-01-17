using DragonSpark.Model.Operations;

namespace DragonSpark.Application.Security.Identity.Authentication.Persist;

sealed class ClearAwarePersist<T> : Appending<PersistInput<T>>, IPersist<T> where T : IdentityUser
{
	public ClearAwarePersist(ClearExistingClaims<T> first, IPersist<T> second) : base(first, second) {}
}