using DragonSpark.Composition.Compose;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Configuration;
using System;

namespace DragonSpark.Diagnostics;

public static class Extensions
{
	extension(BuildHostContext @this)
	{
		public BuildHostContext WithSerilog(bool configure = false) => @this.WithSerilog(x => x, configure);

		public BuildHostContext WithSerilog(Func<LoggerConfiguration, LoggerConfiguration> configuration,
		                                    bool configure = false)
			=> @this.WithSerilog(CreateLoggingProvider.Default.Get, configuration, configure);
		public BuildHostContext WithSerilog(Func<IServiceProvider, ILoggerProvider> provider,
		                                    Func<LoggerConfiguration, LoggerConfiguration> configuration,
		                                    bool configure = false)
			=> @this.Configure(new ConfigureSerilog(provider, configuration, configure));
	}

	[UsedImplicitly]
	public static LoggerConfiguration WithFrameworkEnrichers(this LoggerEnrichmentConfiguration @this)
		=> @this.With(PrimaryAssemblyEnricher.Default, AssemblyDeployInformationEnricher.Default);
}