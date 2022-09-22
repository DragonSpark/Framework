using DragonSpark.Diagnostics.Logging;
using DragonSpark.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Transactions;

public sealed class CurrentDatabaseTransaction : ITransaction
{
	readonly DbContext           _context;
	readonly DatabaseFacade      _facade;
	readonly TransactionNotFound _log;

	public CurrentDatabaseTransaction(DbContext context, TransactionNotFound log)
		: this(context, context.Database, log) {}

	public CurrentDatabaseTransaction(DbContext context, DatabaseFacade facade, TransactionNotFound log)
	{
		_context = context;
		_facade  = facade;
		_log     = log;
	}

	public void Execute(None parameter) {}

	public async ValueTask Get()
	{
		await _context.SaveChangesAsync().ConfigureAwait(false);
		var transaction = _facade.CurrentTransaction;
		if (transaction is not null)
		{
			await transaction.CommitAsync().ConfigureAwait(false);
		}
		else
		{
			_log.Execute(nameof(Get));
		}
	}

	public ValueTask DisposeAsync() => _facade.CurrentTransaction?.DisposeAsync() ?? ValueTask.CompletedTask;

	public sealed class TransactionNotFound : LogWarning<string>
	{
		public TransactionNotFound(ILogger<TransactionNotFound> logger)
			: base(logger, "CurrentTransaction not found for {Action}") {}
	}
}