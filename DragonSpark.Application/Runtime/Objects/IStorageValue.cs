using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Results;

namespace DragonSpark.Application.Runtime.Objects;

public interface IStorageValue<T> : IOperation<T>, IResulting<T?>;