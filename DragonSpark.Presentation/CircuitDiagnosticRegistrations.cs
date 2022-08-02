using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Presentation.Environment;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Presentation;

sealed class CircuitDiagnosticRegistrations : Commands<IServiceCollection>
{
	public static CircuitDiagnosticRegistrations Default { get; } = new();

	CircuitDiagnosticRegistrations()
		: base(Start.A.Command<IServiceCollection>()
		            .By.Calling(x => x.AddSignalR(y => y.AddFilter<ComponentContextFilter>()))
		            .Get(),
		       Registrations.Default, Diagnostics.Registrations.Default) {}
}