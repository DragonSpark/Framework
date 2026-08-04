using DragonSpark.Compose;
using DragonSpark.Composition.Compose;
using JetBrains.Annotations;
using Serilog;
using Serilog.Configuration;

namespace DragonSpark.Diagnostics;

public static class Extensions
{
	extension(BuildHostContext @this)
	{
		public BuildHostContext WithSerilog() => @this.WithSerilog(ConfigureLogging.Default.Execute);

		public BuildHostContext WithSerilog(Action<IServiceProvider, LoggerConfiguration> configure,
		                                    bool preserveExistingLogging = true)
			=> @this.Configure(new ConfigureSerilog(configure, preserveExistingLogging));

		public BuildHostContext WithDeferredLogging() => @this.Configure(ConfigureDeferredLogging.Default);
	}

	[UsedImplicitly]
	public static LoggerConfiguration WithFrameworkEnrichers(this LoggerEnrichmentConfiguration @this)
		=> @this.With(PrimaryAssemblyEnricher.Default, AssemblyDeployInformationEnricher.Default);
}