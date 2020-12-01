using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Connections
{
	sealed class Registrations : ICommand<IServiceCollection>
	{
		public static Registrations Default { get; } = new Registrations();

		Registrations() {}

		public void Execute(IServiceCollection parameter)
		{
			parameter.Start<IHubConnections>()
			         .Forward<HubConnections>()
			         .Scoped()
				;
		}
	}
}