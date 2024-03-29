﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Transactions;

public sealed class ServiceScopedDatabaseTransactions : ITransactions
{
	readonly IServiceScopedTransactions _boundaries;

	public ServiceScopedDatabaseTransactions(IServiceScopedTransactions boundaries) => _boundaries = boundaries;

	public async ValueTask<ITransaction> Get()
	{
		var previous = _boundaries.Get();
		var context  = previous.Provider.GetRequiredService<DbContext>();
		await context.Database.BeginTransactionAsync().ConfigureAwait(false);
		var result = new ServiceScopedDatabaseTransaction(previous, new DatabaseTransaction(context));
		return result;
	}
}