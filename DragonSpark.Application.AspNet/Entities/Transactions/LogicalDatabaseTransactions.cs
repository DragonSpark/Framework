using DragonSpark.Compose;
using DragonSpark.Model.Results;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Transactions;

public sealed class LogicalDatabaseTransactions : ITransactions
{
	public static LogicalDatabaseTransactions Default { get; } = new();

	LogicalDatabaseTransactions() : this(LogicalContext.Default) {}

	readonly IResult<DbContext?> _context;

	public LogicalDatabaseTransactions(IResult<DbContext?> context) => _context = context;

	public async ValueTask<ITransaction> Get()
	{
		var context = _context.Get().Verify();
		await context.Database.BeginTransactionAsync().Await();
		return new RequiredDatabaseTransaction(context);
	}
}