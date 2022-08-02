using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Communication;

sealed class Registrations : ICommand<IServiceCollection>
{
	public static Registrations Default { get; } = new();

	Registrations() {}

	public void Execute(IServiceCollection parameter)
	{
		parameter.Start<IClientStateValue>()
		         .Forward<ClientStateValue>()
		         .Scoped()
		         //
		         .Then.Start<IClientStateValues>()
		         .Forward<ClientStateValues>()
		         .Scoped()
			;
	}
}