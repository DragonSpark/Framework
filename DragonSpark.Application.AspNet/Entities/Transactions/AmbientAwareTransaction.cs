using System.Threading.Tasks;
using DragonSpark.Application.Runtime.Operations.Execution;
using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Results;
using JetBrains.Annotations;

namespace DragonSpark.Application.AspNet.Entities.Transactions;

[MustDisposeResource]
public sealed class AmbientAwareTransaction(
	ITransaction previous,
	IMutable<IOperations?> store,
	IResult<IOperations> instance)
	: ITransaction
{
	readonly ITransaction           _previous = previous;
	readonly IMutable<IOperations?> _store    = store;
	readonly IResult<IOperations>   _instance = instance;

	public AmbientAwareTransaction(ITransaction previous)
		: this(previous, OperationsStore.Default, NewOperations.Default) {}

	public void Execute(None parameter)
	{
		var operations = _instance.Get();
		_store.Execute(operations);
		_previous.Execute(parameter);
	}

	public async ValueTask Get()
	{
		var operations = _store.Get().Verify();
		await _previous.Off();
		await operations.Off();
	}

	public ValueTask DisposeAsync() => _previous.DisposeAsync();
}
