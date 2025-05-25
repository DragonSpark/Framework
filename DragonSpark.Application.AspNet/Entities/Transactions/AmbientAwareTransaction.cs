using DragonSpark.Application.Runtime.Operations.Execution;
using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Results;
using JetBrains.Annotations;
using System.Threading;
using System.Threading.Tasks;

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

	public async ValueTask Get(CancellationToken parameter)
	{
		var operations = _store.Get().Verify();
		await _previous.Off(parameter);
		await operations.Off(parameter);
	}

	public ValueTask DisposeAsync() => _previous.DisposeAsync();
}
