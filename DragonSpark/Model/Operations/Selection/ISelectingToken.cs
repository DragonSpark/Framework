using DragonSpark.Model.Operations.Allocated;

namespace DragonSpark.Model.Operations.Selection;

public interface ISelectingToken<T, TOut> : ISelecting<Token<T>, Token<TOut>>;