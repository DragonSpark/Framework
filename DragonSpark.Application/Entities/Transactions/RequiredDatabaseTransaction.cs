using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Transactions;

sealed class RequiredDatabaseTransaction : ITransaction, IContextAware
{
	readonly DbContext      _context;
	readonly DatabaseFacade _facade;

	public RequiredDatabaseTransaction(DbContext context) : this(context, context.Database) {}

	public RequiredDatabaseTransaction(DbContext context, DatabaseFacade facade)
	{
		_context = context;
		_facade  = facade;
	}

	public void Execute(None parameter) {}

	public async ValueTask Get()
	{
		await _context.SaveChangesAsync().Await();
		await _facade.CurrentTransaction.Verify().CommitAsync().Await();
	}

	public ValueTask DisposeAsync() => _facade.CurrentTransaction?.DisposeAsync() ?? ValueTask.CompletedTask;

	DbContext IResult<DbContext>.Get() => _context;
}