using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Communication;

sealed class RegistrationsUndo : ICommand<IServiceCollection>
{
	public static RegistrationsUndo Default { get; } = new();

	RegistrationsUndo() {}

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