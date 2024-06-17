using DragonSpark.Model.Operations.Allocated;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations;

public interface IOperation<in T> : ISelect<T, ValueTask>;

public interface IOperation : IResult<ValueTask>;

// TODO

public interface ITokenOperation : IOperation<CancellationToken>;
public interface ITokenOperation<T> : IOperation<Token<T>>;