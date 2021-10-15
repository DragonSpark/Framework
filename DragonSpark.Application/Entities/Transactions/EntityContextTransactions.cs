using DragonSpark.Compose;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Transactions;

public sealed class EntityContextTransactions : ITransactions
{
	readonly IScopedTransactions _boundaries;

	public EntityContextTransactions(IScopedTransactions boundaries) => _boundaries = boundaries;

	public ValueTask<ITransaction> Get()
	{
		var previous = _boundaries.Get();
		var context  = previous.Provider.GetRequiredService<DbContext>();
		var second   = new EntityContextTransaction(context);
		var result   = new AppendedTransaction(previous, second).ToOperation<ITransaction>();
		return result;
	}
}