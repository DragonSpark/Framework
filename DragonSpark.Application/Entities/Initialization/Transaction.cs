using DragonSpark.Application.Entities.Transactions;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Initialization;

sealed class Transaction<T> : IOperation where T : DbContext
{
	readonly IServiceProvider _services;
	readonly Type             _initializer;

	public Transaction(IServiceProvider services, Type initializer)
	{
		_services    = services;
		_initializer = initializer;
	}

	public async ValueTask Get()
	{
		var             transactions = _services.GetRequiredService<ServiceScopedDatabaseTransactions>().Ambient();
		await using var session      = await transactions.Get();
		await using var transaction  = session.To<IScopedTransaction>();
		var             initializer  = transaction.Provider.GetRequiredService(_initializer).To<IInitializer>();
		await using var context      = transaction.Provider.GetRequiredService<T>();
		transaction.Execute();
		await initializer.Await(context);
		await transaction.Await();
	}
}