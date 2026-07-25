using DragonSpark.Model.Selection;

namespace DragonSpark.Model.Operations.Allocated;

public interface IAllocating<in T, TOut> : ISelect<T, Task<TOut>>;