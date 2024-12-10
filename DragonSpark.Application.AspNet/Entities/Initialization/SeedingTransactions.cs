using System.Threading.Tasks;
using DragonSpark.Application.AspNet.Entities.Transactions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

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

	[MustDisposeResource]
	public ValueTask<ITransaction> Get()
	{
		var previous = _transactions.Get();
		return new(new ServiceScopedDatabaseTransaction(previous, new EntityContextTransaction(_context)));
	}
}
