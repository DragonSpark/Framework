using DragonSpark.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Transactions;

sealed class DatabaseTransaction : ITransaction
{
	readonly DbContext             _context;
	readonly IDbContextTransaction _transaction;

	public DatabaseTransaction(DbContext context, IDbContextTransaction transaction)
	{
		_context     = context;
		_transaction = transaction;
	}

	public void Execute(None parameter) {}

	public async ValueTask Get()
	{
		await _context.SaveChangesAsync().ConfigureAwait(false);
		await _transaction.CommitAsync().ConfigureAwait(false);
	}

	public ValueTask DisposeAsync() => _transaction.DisposeAsync();
}