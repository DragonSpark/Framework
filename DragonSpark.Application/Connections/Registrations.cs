using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Connections;

sealed class Registrations : ICommand<IServiceCollection>
{
	public static Registrations Default { get; } = new();

	Registrations() {}

	public void Execute(IServiceCollection parameter)
	{
		parameter.Start<SignedHubConnections>()
		         .Singleton()
		         //
		         .Then.Start<IAssignSignedContent>()
		         .Forward<AssignSignedContent>()
		         .Include(x => x.Dependencies.Recursive())
		         .Singleton()
		         //
		         .Then.Start<IHubConnections>()
		         .Forward<HubConnections>()
		         .Scoped()
		         //
		         .Then.Start<IConfigureConnection>()
		         .Forward<ConfigureConnection>()
		         .Scoped();
	}
}