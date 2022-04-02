using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using System.Threading.Tasks;

namespace DragonSpark.Application.Runtime.Operations.Execution;

public sealed class AmbientOperation<TIn, TOut> : ISelecting<TIn, TOut>
{
	readonly ISelecting<TIn, TOut> _previous;
	readonly IMutable<object?>     _store;
	readonly object                _instance;

	public AmbientOperation(ISelecting<TIn, TOut> previous, object instance)
		: this(previous, AmbientOperationInstance.Default, instance) {}

	public AmbientOperation(ISelecting<TIn, TOut> previous, IMutable<object?> store, object instance)
	{
		_previous = previous;
		_store    = store;
		_instance = instance;
	}

	public async ValueTask<TOut> Get(TIn parameter)
	{
		_store.Execute(_instance);
		return await _previous.Await(parameter);
	}
}