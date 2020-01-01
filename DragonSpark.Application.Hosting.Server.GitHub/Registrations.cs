using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using DragonSpark.Services;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Hosting.Server.GitHub
{
	public sealed class DefaultServiceConfiguration : Command<IServiceCollection>, IServiceConfiguration
	{
		[UsedImplicitly]
		public static DefaultServiceConfiguration Default { get; } = new DefaultServiceConfiguration();

		DefaultServiceConfiguration()
			: base(RegisterOption.Of<GitHubApplicationSettings>()
			                     .Then()
			                     .Terminate(Registrations.Default
			                                             .Then(Server.DefaultServiceConfiguration.Default))) {}
	}

	sealed class Registrations : Command<IServiceCollection>
	{
		public static Registrations Default { get; } = new Registrations();

		Registrations() : base(x => x.AddSingleton<EventMessages>()
		                             .AddSingleton<Hasher>()
		                             .AddSingleton<EventMessageBinder>()
		                             .AddSingleton(typeof(FaultAwareTemplate<>))
		                             .AddSingleton(typeof(LoggedProcessorOperation<>))
		                             .AddSingleton(typeof(TaskInformationTemplate<>))
		                             ) {}
	}
}