using DragonSpark.Model.Operations;

namespace DragonSpark.Application.Model.Operations;

public interface IInput<T> : IOperation<UserInput<T>>;

public interface IInput : IOperation<UserInput>;