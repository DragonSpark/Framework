using Serilog.Configuration;

namespace DragonSpark.Diagnostics.Configurations
{
	public abstract class EnrichCommandBase : LoggerConfigurationCommandBase<LoggerEnrichmentConfiguration>
	{
		protected EnrichCommandBase() : base( configuration => configuration.Enrich ) {}
	}
}