using DragonSpark.Model.Operations.Stop;

namespace DragonSpark.Application.Model.Operations;

public interface IInput : IStopAware<UserInput>;

public interface IInput<T> : IStopAware<UserInput<T>>;