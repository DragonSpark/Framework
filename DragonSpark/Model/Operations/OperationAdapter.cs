using System;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations;

public class OperationAdapter : IOperation<None>, IOperation
{
	readonly Func<None, ValueTask> _previous;

	public OperationAdapter(IOperation<None> operation) : this(operation.Get) {}

	public OperationAdapter(IOperation operation) : this(_ => operation.Get()) {}

	public OperationAdapter(Func<None, ValueTask> previous) => _previous = previous;

	public ValueTask Get() => _previous(None.Default);

	public ValueTask Get(None parameter) => _previous(parameter);
}