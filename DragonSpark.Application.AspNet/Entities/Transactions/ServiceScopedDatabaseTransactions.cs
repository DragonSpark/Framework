using DragonSpark.Compose;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Transactions;

public sealed class ServiceScopedDatabaseTransactions(IServiceScopedTransactions transactions) : ITransactions
{
	[MustDisposeResource]
	public async ValueTask<ITransaction> Get(CancellationToken parameter)
	{
		var previous = transactions.Get();
		var context  = previous.Provider.GetRequiredService<DbContext>();
		await context.Database.BeginTransactionAsync(parameter).Off();
		return new ServiceScopedDatabaseTransaction(previous, new DatabaseTransaction(context));
	}
}
