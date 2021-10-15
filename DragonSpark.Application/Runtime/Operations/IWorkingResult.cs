using DragonSpark.Model.Results;

namespace DragonSpark.Application.Runtime.Operations;

public interface IWorkingResult<T> : IResult<Worker<T>> {}

public interface IWorkingResult : IResult<Worker> {}