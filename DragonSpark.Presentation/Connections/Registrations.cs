using DragonSpark.Application.Connections;
using DragonSpark.Composition;
using DragonSpark.Composition.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection.Alterations;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Presentation.Connections
{
	sealed class Registrations : ICommand<IServiceCollection>
	{
		public static Registrations Default { get; } = new Registrations();

		Registrations() {}

		public void Execute(IServiceCollection parameter)
		{
			parameter.Start<IConfigureConnection>()
			         .Forward<ConfigureConnection>()
			         .Scoped();
		}
	}

	sealed class Configure : IAlteration<BuildHostContext>
	{
		public static Configure Default { get; } = new Configure();

		Configure() {}

		public BuildHostContext Get(BuildHostContext parameter) => parameter.Configure(Registrations.Default);
	}
}