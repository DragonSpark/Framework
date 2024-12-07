using DragonSpark.Model.Operations.Selection;

namespace DragonSpark.Application.Model;

public interface IInput<TSubject, TOut> : ISelecting<UserInput<TSubject>, TOut>;

public interface IInput<T> : ISelecting<UserInput, T>;

public interface IInputing<T> : ISelecting<ulong, T>;