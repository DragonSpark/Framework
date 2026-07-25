using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;

namespace DragonSpark.Presentation.Components.State;

sealed class BlockOperation : Operation
{
	public BlockOperation(IOperation operation, Switch monitor)
		: base(new BlockOperation<None>(new OperationAdapter(operation), monitor).Get) {}
}

sealed class BlockOperation<T> : IOperation<T>
{
	readonly IOperation<T> _operation;
	readonly Switch        _monitor;

	public BlockOperation(IOperation<T> operation, Switch monitor)
	{
		_operation = operation;
		_monitor   = monitor;
	}

	public async ValueTask Get(T parameter)
	{
		if (!_monitor)
		{
			using var _ = _monitor.Scoped();
			await _operation.Off(parameter);
		}
	}
}