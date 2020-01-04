using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Hosting.Server.GitHub
{
	sealed class DefaultServiceConfiguration : Command<IServiceCollection>, IServiceConfiguration
	{
		[UsedImplicitly]
		public static DefaultServiceConfiguration Default { get; } = new DefaultServiceConfiguration();

		DefaultServiceConfiguration()
			: base(Start.An.Option<GitHubApplicationSettings>()
			            .Then()
			            .Terminate(Registrations.Default)) {}
	}

	sealed class Registrations : Command<IServiceCollection>
	{
		public static Registrations Default { get; } = new Registrations();

		Registrations() : base(x => x.AddSingleton<EventMessages>()
		                             .AddSingleton<Hasher>()
		                             .AddSingleton<EventMessageBinder>()
		                             //
		                             .ForDefinition<FaultAwareTemplate<object>>()
		                             .Singleton()
		                             //
		                             .ForDefinition<LoggedProcessorOperation<object>>()
		                             .Singleton()
		                             //
		                             .ForDefinition<TaskInformationTemplate<object>>()
		                             .Singleton()
		                      ) {}
	}
}