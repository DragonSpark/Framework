using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using System.Threading.Tasks;

namespace DragonSpark.Application.Runtime.Operations.Execution;

public class DeferredOperation<T> : IOperation<T>
{
	readonly IOperation<T>         _previous;
	readonly IResult<IOperations?> _store;

	public DeferredOperation(IOperation<T> previous) : this(previous, OperationsStore.Default) {}

	public DeferredOperation(IOperation<T> previous, IResult<IOperations?> store)
	{
		_previous = previous;
		_store    = store;
	}

	public ValueTask Get(T parameter)
	{
		var store = _store.Get();
		if (store is not null)
		{
			store.Execute(_previous.Then().Bind(parameter));
			return ValueTask.CompletedTask;
		}

		return _previous.Get(parameter);
	}
}