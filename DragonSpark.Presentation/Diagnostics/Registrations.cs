using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;
using Serilog.Core;

namespace DragonSpark.Presentation.Diagnostics;

sealed class Registrations : ICommand<IServiceCollection>
{
	public static Registrations Default { get; } = new();

	Registrations() {}

	public void Execute(IServiceCollection parameter)
	{
		parameter.Start<ILogEventEnricher>()
		         .Forward<CircuitRecordEnricher>()
		         .Singleton();
	}
}