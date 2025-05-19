using DragonSpark.Model.Operations.Selection;
using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Application.Model;

public interface IInput<TSubject, TOut> : ISelecting<UserInput<TSubject>, TOut>;
public interface IInput<T> : ISelecting<UserInput, T>;
public interface IInputing<T> : ISelecting<ulong, T>; // TODO: Fix/Rename

// TODO: Temp, or not?
public interface IInputWithStop<TSubject, TOut> : IStopAware<UserInput<TSubject>, TOut>;
public interface IInputWithStop<T> : IStopAware<UserInput, T>;
public interface IInputtingWithStop<T> : IStopAware<ulong, T>;