namespace DragonSpark.Application.Runtime.Objects;

public interface IStorageValue<T> : DragonSpark.Model.Operations.IStopAware<T>,
                                    DragonSpark.Model.Operations.Results.IStopAware<T?>;