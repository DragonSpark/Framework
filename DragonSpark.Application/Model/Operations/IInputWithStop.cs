using DragonSpark.Model.Operations;

namespace DragonSpark.Application.Model.Operations;

public interface IInputWithStop : IOperation<Stop<UserInput>>;

public interface IInputWithStop<T> : IOperation<Stop<UserInput<T>>>;