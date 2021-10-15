using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;

namespace DragonSpark.Model.Sequences;

public interface IArray<T> : IResult<Array<T>> {}

public interface IArray<in _, T> : ISelect<_, Array<T>> {}