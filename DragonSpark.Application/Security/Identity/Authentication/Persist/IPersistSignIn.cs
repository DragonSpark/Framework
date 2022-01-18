using DragonSpark.Model.Operations;

namespace DragonSpark.Application.Security.Identity.Authentication.Persist;

public interface IPersistSignIn<T> : IOperation<PersistInput<T>> where T : IdentityUser {}