using System.Threading.Tasks;
using DragonSpark.Compose;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.AspNet.Entities.Transactions;

public sealed class ServiceScopedDatabaseTransactions(IServiceScopedTransactions transactions) : ITransactions
{
	[MustDisposeResource]
	public async ValueTask<ITransaction> Get()
	{
		var previous = transactions.Get();
		var context  = previous.Provider.GetRequiredService<DbContext>();
		await context.Database.BeginTransactionAsync().Await();
		return new ServiceScopedDatabaseTransaction(previous, new DatabaseTransaction(context));
	}
}
