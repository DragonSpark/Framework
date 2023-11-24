using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Connections.Client;

sealed class Registrations : ICommand<IServiceCollection>
{
	public static Registrations Default { get; } = new();

	Registrations() {}

	public void Execute(IServiceCollection parameter)
	{
		parameter.Start<ConfigureConnection>()
		         .Include(x => x.Dependencies.Recursive())
		         .Scoped()
		         .Then.Decorate<IConfigureConnection, ConfigureConnection>()
		         //
		         .Start<IRestartConnection>()
		         .Forward<RestartConnection>()
		         .Decorate<PolicyAwareRestartConnection>()
		         .Singleton()
			;
	}
}