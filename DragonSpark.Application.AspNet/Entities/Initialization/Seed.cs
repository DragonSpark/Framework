﻿using DragonSpark.Application.AspNet.Entities.Transactions;
using DragonSpark.Compose;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Initialization;

sealed class Seed : ISeed
{
	readonly Type _initializer;

	public Seed(Type initializer) => _initializer = initializer;

	public async ValueTask Get(SeedInput parameter)
	{
		var (services, context) = parameter;
		var             previous    = services.GetRequiredService<IServiceScopedTransactions>();
		await using var session     = await new SeedingTransactions(context, previous).Await();
		await using var scoped      = session.To<IScopedTransaction>();
		var             initializer = scoped.Provider.GetRequiredService(_initializer).To<IInitializer>();
		var             transaction = new AmbientAwareTransaction(scoped);
		transaction.Execute();
		await initializer.Await(context);
		await transaction.Await();
	}
}