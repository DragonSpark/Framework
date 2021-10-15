using DragonSpark.Application.Connections;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Presentation.Connections;

sealed class Registrations : ICommand<IServiceCollection>
{
	public static Registrations Default { get; } = new Registrations();

	Registrations() {}

	public void Execute(IServiceCollection parameter)
	{
		parameter.Start<IConfigureConnection>()
		         .Forward<ConfigureConnection>()
		         .Scoped()
			//
			/*.Then.Start<ClientIdentifier>()
			.Include(x => x.Dependencies)
			.Scoped()
			//
			.Then.Decorate<IClientIdentifier, ClientIdentifier>()*/
			;
	}
}