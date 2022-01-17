using DragonSpark.Model.Operations;

namespace DragonSpark.Application.Security.Identity.Authentication.Persist;

public interface IPersistClaims<T> : IOperation<Claims<T>> {}