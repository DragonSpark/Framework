using DragonSpark.Application.AspNet.Entities.Transactions;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Initialization;

sealed class SeedingTransactions : ITransactions
{
	readonly DbContext                  _context;
	readonly IServiceScopedTransactions _transactions;

	public SeedingTransactions(DbContext context, IServiceScopedTransactions transactions)
	{
		_context      = context;
		_transactions = transactions;
	}

	public ValueTask<ITransaction> Get()
	{
		var previous    = _transactions.Get();
		var transaction = new ServiceScopedDatabaseTransaction(previous, new EntityContextTransaction(_context));
		return new(transaction);
	}
}