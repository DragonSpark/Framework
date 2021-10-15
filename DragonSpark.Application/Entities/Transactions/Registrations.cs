using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Entities.Transactions;

sealed class Registrations : ICommand<IServiceCollection>
{
	public static Registrations Default { get; } = new();

	Registrations() {}

	public void Execute(IServiceCollection parameter)
	{
		parameter.Start<IScopedTransactions>()
		         .Forward<ScopedTransactions>()
		         .Singleton()
		         //
		         .Then.Start<ITransactions>()
		         .Forward<Transactions>()
		         .Singleton()
		         //
		         .Then.Start<EntityContextTransactions>()
		         .And<ServiceScopedDatabaseTransactions>()
		         .Singleton()
		         //
		         .Then.Start<ScopedEntityContextTransactions>()
		         .Scoped()
			;
	}
}