using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Results;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.AspNet.Entities.Transactions;

public sealed class LogicalDatabaseTransactions : ITransactions
{
	public static LogicalDatabaseTransactions Default { get; } = new();

	LogicalDatabaseTransactions() : this(LogicalContext.Default) {}

	readonly IResult<DbContext?> _context;

	public LogicalDatabaseTransactions(IResult<DbContext?> context) => _context = context;

	[MustDisposeResource]
	public async ValueTask<ITransaction> Get()
	{
		var context = _context.Get().Verify();
		await context.Database.BeginTransactionAsync().Off();
		return new RequiredDatabaseTransaction(context);
	}
}
