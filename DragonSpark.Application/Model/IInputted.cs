using DragonSpark.Model.Operations;

namespace DragonSpark.Application.Model;

public interface IInputted<T> : IOperation<UserInput<T>>;

public interface IInputted : IOperation<UserInput>;

// TODO
public interface IInputtedWithStop<T> : IOperation<Stop<UserInput<T>>>;
public interface IInputtedWithStop : IOperation<Stop<UserInput>>;