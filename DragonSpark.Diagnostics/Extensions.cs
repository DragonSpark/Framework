using DragonSpark.Composition.Compose;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Configuration;
using System;

namespace DragonSpark.Diagnostics;

public static class Extensions
{
	public static BuildHostContext WithSerilog(this BuildHostContext @this)
		=> @this.WithSerilog(CreateLoggingProvider.Default.Get);

	public static BuildHostContext WithSerilog(this BuildHostContext @this,
	                                           Func<IServiceProvider, ILoggerProvider> provider)
		=> @this.Configure(new ConfigureSerilog(provider));

	[UsedImplicitly]
	public static LoggerConfiguration WithFrameworkEnrichers(this LoggerEnrichmentConfiguration @this)
		=> @this.With(PrimaryAssemblyEnricher.Default, AssemblyDeployInformationEnricher.Default);
}