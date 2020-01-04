﻿using DragonSpark.Composition;
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
			: base(RegisterOption.Of<GitHubApplicationSettings>()
			                     .Then()
			                     .Terminate(Registrations.Default)) {}
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