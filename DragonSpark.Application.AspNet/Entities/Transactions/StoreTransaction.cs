using System.Threading.Tasks;
using DragonSpark.Model;
using DragonSpark.Model.Results;
using JetBrains.Annotations;

namespace DragonSpark.Application.AspNet.Entities.Transactions;

[MustDisposeResource]
abstract class StoreTransaction<T>(T value, IMutable<T?> store) : ITransaction
{
	public void Execute(None parameter)
	{
		store.Execute(value);
	}

	public ValueTask Get() => ValueTask.CompletedTask;

	public ValueTask DisposeAsync()
	{
		store.Execute(default);
		return ValueTask.CompletedTask;
	}
}
