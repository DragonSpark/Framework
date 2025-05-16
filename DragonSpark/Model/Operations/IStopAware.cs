using System.Threading;

namespace DragonSpark.Model.Operations;

public interface IStopAware<T> : IOperation<Stop<T>>;

public interface IStopAware : IOperation<CancellationToken>;