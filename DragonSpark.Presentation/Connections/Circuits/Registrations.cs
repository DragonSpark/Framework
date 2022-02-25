using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Components.Server.Circuits;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Presentation.Connections.Circuits;

public sealed class Registrations : ICommand<IServiceCollection>
{
	public static Registrations Default { get; } = new();

	Registrations() {}

	public void Execute(IServiceCollection parameter)
	{
		parameter.Start<CreateCircuitRecord>()
		         .Scoped()
		         //
		         .Then.Start<ClearCircuit>()
		         .Include(x => x.Dependencies.Recursive())
		         .Singleton()
		         //
		         .Then.Start<CircuitHandler>()
		         .Forward<RecordAwareCircuitHandler>()
		         .Include(x => x.Dependencies)
		         .Scoped()
			;
	}
}