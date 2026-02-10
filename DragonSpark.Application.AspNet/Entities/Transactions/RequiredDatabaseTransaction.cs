using System.Threading;
using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Results;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DragonSpark.Application.AspNet.Entities.Transactions;

[MustDisposeResource]
sealed class RequiredDatabaseTransaction(DbContext context, DatabaseFacade facade) : ITransaction, IContextAware
{
	public RequiredDatabaseTransaction(DbContext context) : this(context, context.Database) {}

	public void Execute(None parameter) {}

	public async ValueTask Get(CancellationToken parameter)
	{
		await context.SaveChangesAsync(parameter).Off();
		await facade.CurrentTransaction.Verify().CommitAsync(parameter).Off();
	}

	public ValueTask DisposeAsync() => facade.CurrentTransaction?.DisposeAsync() ?? ValueTask.CompletedTask;

	DbContext IResult<DbContext>.Get() => context;
}
