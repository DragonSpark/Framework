using DragonSpark.Model.Operations.Allocated;
using System.Threading;

namespace DragonSpark.Model.Operations;

public interface IStopAware<T> : IOperation<Token<T>>;

public interface IStopAware : IOperation<CancellationToken>;