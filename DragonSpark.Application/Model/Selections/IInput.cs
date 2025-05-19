using DragonSpark.Model.Operations.Selection;

namespace DragonSpark.Application.Model.Selections;

public interface IInput<TSubject, TOut> : ISelecting<UserInput<TSubject>, TOut>;
public interface IInput<T> : ISelecting<UserInput, T>;