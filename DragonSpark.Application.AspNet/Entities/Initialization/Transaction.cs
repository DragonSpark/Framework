using DragonSpark.Application.AspNet.Entities.Transactions;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Initialization;

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
		var             transactions = _services.GetRequiredService<ServiceScopedDatabaseTransactions>();
		await using var session      = await transactions.Get();
		await using var scoped       = session.To<IScopedTransaction>();
		var             initializer  = scoped.Provider.GetRequiredService(_initializer).To<IInitializer>();
		await using var context      = scoped.Provider.GetRequiredService<T>();
		var             transaction  = new AmbientAwareTransaction(scoped);
		transaction.Execute();
		await initializer.Await(context);
		await transaction.Await();
	}
}