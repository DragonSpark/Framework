using DragonSpark.Model.Operations.Selection;
using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Application.Model;

public interface IInput<TSubject, TOut> : ISelecting<UserInput<TSubject>, TOut>;
// TODO: Temp, or not?
public interface IInputWithStop<TSubject, TOut> : IStopAware<UserInput<TSubject>, TOut>;

public interface IInput<T> : ISelecting<UserInput, T>;

public interface IInputing<T> : ISelecting<ulong, T>;