using DragonSpark.Model.Operations;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication.Persist;

public interface IPersistSignInWithMetadata<T> : IOperation<PersistMetadataInput<T>>;