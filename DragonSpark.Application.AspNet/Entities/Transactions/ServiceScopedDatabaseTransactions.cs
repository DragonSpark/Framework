using DragonSpark.Compose;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Transactions;

public sealed class ServiceScopedDatabaseTransactions : ITransactions
{
	readonly IServiceScopedTransactions _transactions;

	public ServiceScopedDatabaseTransactions(IServiceScopedTransactions transactions) => _transactions = transactions;

	public async ValueTask<ITransaction> Get()
	{
		var previous = _transactions.Get();
		var context  = previous.Provider.GetRequiredService<DbContext>();
		await context.Database.BeginTransactionAsync().Await();
		return new ServiceScopedDatabaseTransaction(previous, new DatabaseTransaction(context));
	}
}