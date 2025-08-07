using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Model.Operations.Stop;

namespace DragonSpark.Application.Runtime.Objects;

public interface IStorageValue<T>
    : IStopAware<T>, IDepending, DragonSpark.Model.Operations.Results.Stop.IStopAware<T?>;