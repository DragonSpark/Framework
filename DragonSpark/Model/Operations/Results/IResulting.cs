using DragonSpark.Model.Results;

namespace DragonSpark.Model.Operations.Results;

public interface IResulting<T> : IResult<ValueTask<T>>;