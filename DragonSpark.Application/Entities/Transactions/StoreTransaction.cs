using DragonSpark.Model;
using DragonSpark.Model.Results;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Transactions;

class StoreTransaction<T> : ITransaction
{
	readonly T            _value;
	readonly IMutable<T?> _store;

	protected StoreTransaction(T value, IMutable<T?> store)
	{
		_value = value;
		_store = store;
	}

	public void Execute(None parameter)
	{
		_store.Execute(_value);
	}

	public ValueTask Get() => ValueTask.CompletedTask;

	public ValueTask DisposeAsync()
	{
		_store.Execute(default);
		return ValueTask.CompletedTask;
	}
}