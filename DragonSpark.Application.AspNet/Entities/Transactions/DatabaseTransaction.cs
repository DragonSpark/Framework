using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DragonSpark.Application.AspNet.Entities.Transactions;

[MustDisposeResource]
sealed class DatabaseTransaction(DbContext context, DatabaseFacade facade) : ITransaction
{
	public DatabaseTransaction(DbContext context) : this(context, context.Database) {}

	public void Execute(None parameter) {}

	public async ValueTask Get()
	{
		await context.SaveChangesAsync().Off();
		var transaction = facade.CurrentTransaction;
		if (transaction is not null)
		{
			await transaction.CommitAsync().Off();
		}
	}

	public ValueTask DisposeAsync() => facade.CurrentTransaction?.DisposeAsync() ?? ValueTask.CompletedTask;
}
