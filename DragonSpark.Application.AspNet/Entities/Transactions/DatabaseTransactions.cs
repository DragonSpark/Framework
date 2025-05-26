using DragonSpark.Compose;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Transactions;

public sealed class DatabaseTransactions(DbContext context, DatabaseFacade facade) : ITransactions
{
	public DatabaseTransactions(DbContext owner) : this(owner, owner.Database) {}

	[MustDisposeResource]
	public async ValueTask<ITransaction> Get(CancellationToken parameter)
	{
		await facade.BeginTransactionAsync(parameter).Off();
		return new DatabaseTransaction(context);
	}
}
