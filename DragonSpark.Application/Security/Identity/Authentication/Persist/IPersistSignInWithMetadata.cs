using DragonSpark.Model.Operations;

namespace DragonSpark.Application.Security.Identity.Authentication.Persist;

public interface IPersistSignInWithMetadata<T> : IOperation<PersistMetadataInput<T>> {}