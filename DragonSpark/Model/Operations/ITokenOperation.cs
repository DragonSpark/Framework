using DragonSpark.Model.Operations.Allocated;
using System.Threading;

namespace DragonSpark.Model.Operations;

public interface ITokenOperation<T> : IOperation<Token<T>>;

public interface ITokenOperation : IOperation<CancellationToken>;