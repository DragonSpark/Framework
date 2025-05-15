using DragonSpark.Model.Operations.Allocated;

namespace DragonSpark.Model.Operations.Selection;

public interface IStoppingAware<TIn, TOut> : ISelecting<Token<TIn>, TOut>;