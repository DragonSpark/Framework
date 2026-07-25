using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;

namespace DragonSpark.Model.Operations.Allocated;

public interface IAllocated : IResult<Task>;

public interface IAllocated<in T> : ISelect<T, Task>;