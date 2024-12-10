using System.Threading.Tasks;
using DragonSpark.Compose;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DragonSpark.Application.AspNet.Entities.Transactions;

public sealed class DatabaseTransactions(DbContext context, DatabaseFacade facade) : ITransactions
{
	public DatabaseTransactions(DbContext owner) : this(owner, owner.Database) {}

	[MustDisposeResource]
	public async ValueTask<ITransaction> Get()
	{
		await facade.BeginTransactionAsync().Await();
		return new DatabaseTransaction(context);
	}
}
