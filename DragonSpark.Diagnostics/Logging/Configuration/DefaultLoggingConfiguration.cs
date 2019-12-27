using DragonSpark.Diagnostics.Logging.Enrichment;

namespace DragonSpark.Diagnostics.Logging.Configuration
{
	sealed class DefaultLoggingConfiguration : LoggerConfigurations
	{
		public static DefaultLoggingConfiguration Default { get; } = new DefaultLoggingConfiguration();

		DefaultLoggingConfiguration() : base(LoggingLevelControllerConfiguration.Default,
		                                     new EnrichmentConfiguration(PrimaryAssemblyEnricher.Default).ToConfiguration(),
											 EnhancedExceptionStackTraceConfiguration.Default.ToConfiguration(),
		                                     LogContextEnricher.Default.ToConfiguration()) {}
	}
}