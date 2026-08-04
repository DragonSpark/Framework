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
		public BuildHostContext WithSerilog(bool preserveOutputs = true)
			=> @this.WithSerilog(ConfigureLogging.Default.Execute, preserveOutputs);

		public BuildHostContext WithSerilog(Action<IServiceProvider, LoggerConfiguration> configure,
		                                    bool preserveOutputs = true)
			=> @this.Configure(new ConfigureSerilog(configure, preserveOutputs));

		public BuildHostContext WithSerilogUsingDeferredLogging(bool preserveOutputs = true)
			=> @this.Configure(new ConfigureDeferredLogging(preserveOutputs));
	}

	[UsedImplicitly]
	public static LoggerConfiguration WithFrameworkEnrichers(this LoggerEnrichmentConfiguration @this)
		=> @this.With(PrimaryAssemblyEnricher.Default, AssemblyDeployInformationEnricher.Default);
}