using DragonSpark.Compose;
using DragonSpark.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Transactions;

sealed class DatabaseTransaction : ITransaction
{
	readonly DbContext      _context;
	readonly DatabaseFacade _facade;

	public DatabaseTransaction(DbContext context) : this(context, context.Database) {}

	public DatabaseTransaction(DbContext context, DatabaseFacade facade)
	{
		_context     = context;
		_facade = facade;
	}

	public void Execute(None parameter) {}

	public async ValueTask Get()
	{
		await _context.SaveChangesAsync().Await();
		var transaction = _facade.CurrentTransaction;
		if (transaction is not null)
		{
			await transaction.CommitAsync().Await();
		}
	}

	public ValueTask DisposeAsync() => _facade.CurrentTransaction?.DisposeAsync() ?? ValueTask.CompletedTask;
}