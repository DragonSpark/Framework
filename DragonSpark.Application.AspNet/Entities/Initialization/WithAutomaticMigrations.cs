using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.AspNet.Entities.Initialization;

sealed class WithAutomaticMigrations : ICommand<IServiceCollection>
{
	public static WithAutomaticMigrations Default { get; } = new();

	WithAutomaticMigrations() {}

	public void Execute(IServiceCollection parameter)
	{
		parameter.Register<AutomaticMigrationSettings>().Start<IInitialize>().Forward<AutomaticMigration>().Singleton();
	}
}