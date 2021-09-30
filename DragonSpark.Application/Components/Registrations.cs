using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Components
{
	sealed class Registrations : ICommand<IServiceCollection>
	{
		public static Registrations Default { get; } = new();

		Registrations() {}

		public void Execute(IServiceCollection parameter)
		{
			parameter.Start<ConnectionIdentifier>()
			         .And<ConnectionStartTime>()
			         .Scoped()
			         //
			         .Then.Start<IClientIdentifier>()
			         .Forward<ClientIdentifier>()
			         .Scoped()
				//
				;
		}
	}
}