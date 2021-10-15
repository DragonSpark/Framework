using DragonSpark.Model.Commands;

namespace DragonSpark.Model.Results;

public interface IMutable<T> : IResult<T>, ICommand<T> {}