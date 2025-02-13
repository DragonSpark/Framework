using System;
using DragonSpark.Composition.Compose;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Configuration;

namespace DragonSpark.Diagnostics;

public static class Extensions
{
	public static BuildHostContext WithSerilog(this BuildHostContext @this, bool configure = false)
		=> @this.WithSerilog(CreateLoggingProvider.Default.Get, configure);

	public static BuildHostContext WithSerilog(this BuildHostContext @this,
	                                           Func<IServiceProvider, ILoggerProvider> provider, bool configure = false)
		=> @this.Configure(new ConfigureSerilog(provider, configure));

	[UsedImplicitly]
	public static LoggerConfiguration WithFrameworkEnrichers(this LoggerEnrichmentConfiguration @this)
		=> @this.With(PrimaryAssemblyEnricher.Default, AssemblyDeployInformationEnricher.Default);
}