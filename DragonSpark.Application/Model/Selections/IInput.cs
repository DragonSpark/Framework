using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Application.Model.Selections;

public interface IInput<T> : IStopAware<UserInput, T>;

public interface IInput<TSubject, TOut> : IStopAware<UserInput<TSubject>, TOut>;