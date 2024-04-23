using DragonSpark.Model.Operations;

namespace DragonSpark.Application.Model;

public interface IInputted<T> : IOperation<UserInput<T>>;