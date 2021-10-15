using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Transactions;

public class SessionDatabaseTransactions : ITransactions
{
	readonly DbContext      _context;
	readonly DatabaseFacade _facade;

	public SessionDatabaseTransactions(DbContext owner) : this(owner, owner.Database) {}

	public SessionDatabaseTransactions(DbContext context, DatabaseFacade facade)
	{
		_context = context;
		_facade  = facade;
	}

	public async ValueTask<ITransaction> Get()
	{
		var transaction = await _facade.BeginTransactionAsync().ConfigureAwait(false);
		return new DatabaseTransaction(_context, transaction);
	}
}