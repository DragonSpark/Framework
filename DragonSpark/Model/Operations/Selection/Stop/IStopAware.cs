namespace DragonSpark.Model.Operations.Selection.Stop;

public interface IStopAware<TIn, TOut> : ISelecting<Stop<TIn>, TOut>;

// TODO

public interface IAltering<T> : IStopAware<T, T>;