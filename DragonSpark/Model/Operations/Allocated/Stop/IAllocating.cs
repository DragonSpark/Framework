using DragonSpark.Model.Selection;

namespace DragonSpark.Model.Operations.Allocated.Stop;

public interface IAllocating<T, TOut> : ISelect<Stop<T>, Task<TOut>>;