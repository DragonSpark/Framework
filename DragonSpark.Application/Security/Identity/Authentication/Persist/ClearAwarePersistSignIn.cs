using DragonSpark.Model.Operations;

namespace DragonSpark.Application.Security.Identity.Authentication.Persist;

sealed class ClearAwarePersistSignIn<T> : Appending<PersistInput<T>>, IPersistSignIn<T> where T : IdentityUser
{
	public ClearAwarePersistSignIn(ClearExistingClaims<T> first, IPersistSignIn<T> second) : base(first, second) {}
}