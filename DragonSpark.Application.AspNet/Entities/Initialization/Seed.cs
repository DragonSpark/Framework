using DragonSpark.Application.AspNet.Entities.Transactions;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Initialization;

sealed class Seed : ISeed
{
	readonly Type _initializer;

	public Seed(Type initializer) => _initializer = initializer;

	public async ValueTask Get(Stop<SeedInput> parameter)
	{
		var ((services, context), stop) = parameter;
		var             previous    = services.GetRequiredService<IServiceScopedTransactions>();
		await using var session     = await new SeedingTransactions(context, previous).Off(stop);
		await using var scoped      = session.To<IScopedTransaction>();
		var             initializer = scoped.Provider.GetRequiredService(_initializer).To<IInitializer>();
		await using var transaction = new AmbientAwareTransaction(scoped);
		transaction.Execute();
		await initializer.Off(new(context, stop));
		await transaction.Off(stop);
	}
}