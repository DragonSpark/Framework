using DragonSpark.Model.Operations;

namespace DragonSpark.Application.Runtime;

public interface IThrottling<T> : IOperation<Throttle<T>> {}