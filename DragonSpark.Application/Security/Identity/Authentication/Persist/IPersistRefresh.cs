using DragonSpark.Model.Operations;

namespace DragonSpark.Application.Security.Identity.Authentication.Persist;

public interface IPersistRefresh<T> : IOperation<PersistMetadataInput<T>> {}