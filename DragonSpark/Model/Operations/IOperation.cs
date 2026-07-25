using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;

namespace DragonSpark.Model.Operations;

public interface IOperation<in T> : ISelect<T, ValueTask>;

public interface IOperation : IResult<ValueTask>;