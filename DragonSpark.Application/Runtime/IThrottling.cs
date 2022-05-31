using DragonSpark.Model.Commands;

namespace DragonSpark.Application.Runtime;

public interface IThrottling<T> : ICommand<Throttle<T>> {}

/*public interface IThrottling : ICommand<Operate> {}*/