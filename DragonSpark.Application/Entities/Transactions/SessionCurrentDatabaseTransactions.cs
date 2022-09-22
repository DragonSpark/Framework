using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Transactions;

public class SessionCurrentDatabaseTransactions : ITransactions
{
	readonly DbContext                                      _context;
	readonly DatabaseFacade                                 _facade;
	readonly CurrentDatabaseTransaction.TransactionNotFound _log;

	public SessionCurrentDatabaseTransactions(DbContext owner, CurrentDatabaseTransaction.TransactionNotFound log)
		: this(owner, owner.Database, log) {}

	public SessionCurrentDatabaseTransactions(DbContext context, DatabaseFacade facade, CurrentDatabaseTransaction.TransactionNotFound log)
	{
		_context = context;
		_facade  = facade;
		_log     = log;
	}

	public async ValueTask<ITransaction> Get()
	{
		await _facade.BeginTransactionAsync().ConfigureAwait(false);
		return new CurrentDatabaseTransaction(_context, _facade, _log);
	}
}