using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection;

namespace DragonSpark.Application.Runtime.Objects;

public interface IStorageValue<T>
    : IStopAware<T>, IStopAwareDepending, DragonSpark.Model.Operations.Results.IStopAware<T?>;