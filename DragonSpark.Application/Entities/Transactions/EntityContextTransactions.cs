using DragonSpark.Compose;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Transactions;

public sealed class EntityContextTransactions : ITransactions
{
	readonly IServiceScopedTransactions _previous;

	public EntityContextTransactions(IServiceScopedTransactions previous) => _previous = previous;

	public ValueTask<ITransaction> Get()
	{
		var previous = _previous.Get();
		var context  = previous.Provider.GetRequiredService<DbContext>();
		var second   = new EntityContextTransaction(context);
		var result   = new AppendedTransaction(previous, second).ToOperation<ITransaction>();
		return result;
	}
}

// TODO

public sealed class StandardEntityContextTransactions : ITransactions
{
	readonly IServiceScopedTransactions _previous;

	public StandardEntityContextTransactions(IServiceScopedTransactions previous) => _previous = previous;

	public ValueTask<ITransaction> Get()
	{
		var previous = _previous.Get();
		var context  = previous.Provider.GetRequiredService<DbContext>();
		var appended = new AppendedTransaction(previous, new AssignAmbientEntityContextTransaction(context));
		return appended.ToOperation<ITransaction>();
	}
}