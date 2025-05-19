using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Application.Model.Selections;

public interface IInputWithStop<T> : IStopAware<UserInput, T>;

public interface IInputWithStop<TSubject, TOut> : IStopAware<UserInput<TSubject>, TOut>;