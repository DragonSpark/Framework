using DragonSpark.Model.Operations;

namespace DragonSpark.Application.Security.Identity.Authentication.Persist;

public interface IPersist<T> : IOperation<PersistInput<T>> where T : IdentityUser {}