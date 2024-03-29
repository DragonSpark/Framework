using DragonSpark.Application.Runtime.Operations.Execution;
using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Results;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Transactions;

public sealed class AmbientAwareTransaction : ITransaction
{
	readonly ITransaction           _previous;
	readonly IMutable<IOperations?> _store;
	readonly IResult<IOperations>   _instance;

	public AmbientAwareTransaction(ITransaction previous)
		: this(previous, OperationsStore.Default, NewOperations.Default) {}

	public AmbientAwareTransaction(ITransaction previous, IMutable<IOperations?> store, IResult<IOperations> instance)
	{
		_previous = previous;
		_store    = store;
		_instance = instance;
	}

	public void Execute(None parameter)
	{
		var operations = _instance.Get();
		_store.Execute(operations);
		_previous.Execute(parameter);
	}

	public async ValueTask Get()
	{
		var operations = _store.Get().Verify();
		await _previous.Await();
		await operations.Await();
	}

	public ValueTask DisposeAsync() => _previous.DisposeAsync();
}