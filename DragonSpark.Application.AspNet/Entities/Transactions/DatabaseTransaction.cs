using DragonSpark.Compose;
using DragonSpark.Model;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Transactions;

[MustDisposeResource]
sealed class DatabaseTransaction(DbContext context, DatabaseFacade facade) : ITransaction
{
	public DatabaseTransaction(DbContext context) : this(context, context.Database) {}

	public void Execute(None parameter) {}

	public async ValueTask Get(CancellationToken parameter)
	{
		await context.SaveChangesAsync(parameter).Off();
		var transaction = facade.CurrentTransaction;
		if (transaction is not null)
		{
			await transaction.CommitAsync(parameter).Off();
		}
	}

	public ValueTask DisposeAsync() => facade.CurrentTransaction?.DisposeAsync() ?? ValueTask.CompletedTask;
}
