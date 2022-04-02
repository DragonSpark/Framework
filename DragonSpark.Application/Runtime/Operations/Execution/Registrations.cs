using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Runtime.Operations.Execution;

sealed class Registrations : ICommand<IServiceCollection>
{
	public static Registrations Default { get; } = new();

	Registrations() {}

	public void Execute(IServiceCollection parameter)
	{
		parameter.Start<IInstanceOperations>()
		         .Forward<InstanceOperations>()
		         .Singleton()
		         //
		         .Then.Start<IAmbientOperations>()
		         .Forward<AmbientOperations>()
		         .Singleton()
			//
			;
	}
}