using DragonSpark.Model.Operations.Allocated;

namespace DragonSpark.Model.Operations.Selection;

public interface IContinuing<T, TOut> : ISelecting<Token<T>, Token<TOut>>;