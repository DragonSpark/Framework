using DragonSpark.Model.Commands;
using DragonSpark.Presentation.Environment;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Presentation;

sealed class CircuitDiagnosticRegistrations : Commands<IServiceCollection>
{
	public static CircuitDiagnosticRegistrations Default { get; } = new();

	CircuitDiagnosticRegistrations()
		: base(AddSignalR.Default, Registrations.Default, Diagnostics.Registrations.Default) {}
}