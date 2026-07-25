using DragonSpark.Model.Results;

namespace DragonSpark.Model.Operations.Allocated;

public interface IAllocatedResult<T> : IResult<Task<T>>;