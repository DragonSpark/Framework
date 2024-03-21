using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Transactions;

public sealed class DatabaseTransactions : ITransactions
{
	readonly DbContext      _context;
	readonly DatabaseFacade _facade;

	public DatabaseTransactions(DbContext owner) : this(owner, owner.Database) {}

	public DatabaseTransactions(DbContext context, DatabaseFacade facade)
	{
		_context = context;
		_facade  = facade;
	}

	public async ValueTask<ITransaction> Get()
	{
		await _facade.BeginTransactionAsync().ConfigureAwait(false);
		return new DatabaseTransaction(_context);
	}
}