using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using DragonSpark.Presentation.Environment;
using Microsoft.AspNetCore.Components.Server.Circuits;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Presentation.Connections.Circuits;

sealed class Registrations : ICommand<IServiceCollection>
{
	public static Registrations Default { get; } = new();

	Registrations() {}

	public void Execute(IServiceCollection parameter)
	{
		parameter.Start<CreateCircuitRecord>()
		         .And<CurrentCircuitStore>()
		         .And<ApplyState>()
		         .Scoped()
		         .Then.Start<ClientStateAwareInitializeConnection>()
		         .Include(x => x.Dependencies.Recursive())
		         .Scoped()
		         //
		         .Then.Start<CircuitHandler>()
		         .Forward<RecordAwareCircuitHandler>()
		         .Include(x => x.Dependencies.Recursive())
		         .Scoped()
		         //
		         .Then.Decorate<IInitializeConnection, CircuitAwareInitializeConnection>()
		         .Decorate<IInitializeConnection, ClientStateAwareInitializeConnection>()
		         .Decorate<IDetermineContext, ClientStateAwareDetermineContext>()
			;
	}
}