using System;
using System.Threading.Tasks;
using DragonSpark.Application.AspNet.Entities.Transactions;
using DragonSpark.Compose;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.AspNet.Entities.Initialization;

sealed class Seed : ISeed
{
	readonly Type _initializer;

	public Seed(Type initializer) => _initializer = initializer;

	public async ValueTask Get(SeedInput parameter)
	{
		var (services, context) = parameter;
		var             previous    = services.GetRequiredService<IServiceScopedTransactions>();
		await using var session     = await new SeedingTransactions(context, previous).Off();
		await using var scoped      = session.To<IScopedTransaction>();
		var             initializer = scoped.Provider.GetRequiredService(_initializer).To<IInitializer>();
		await using var transaction = new AmbientAwareTransaction(scoped);
		transaction.Execute();
		await initializer.Off(context);
		await transaction.Off();
	}
}