using Serilog.Configuration;

namespace DragonSpark.Diagnostics.Configurations
{
	public abstract class EnrichCommandBase : ConfigureLoggerBase<LoggerEnrichmentConfiguration>
	{
		protected EnrichCommandBase() : base( configuration => configuration.Enrich ) {}
	}
}