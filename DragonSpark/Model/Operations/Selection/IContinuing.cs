namespace DragonSpark.Model.Operations.Selection;

public interface IContinuing<T, TOut> : ISelecting<Stop<T>, Stop<TOut>>;