using DragonSpark.Model.Selection;

namespace DragonSpark.Model.Operations.Allocated.Stop;

public interface IAllocated<T, TOut> : ISelect<Stop<T>, Task<TOut>>;

public interface IAllocated<T> : ISelect<Stop<T>, Task>;
public interface IAllocated : ISelect<CancellationToken, Task>;