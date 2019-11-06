using Serilog;
using Serilog.Configuration;
using Serilog.Enrichers.Correlation;
using DragonSpark.Diagnostics.Logging.Configuration;
using DragonSpark.Model.Selection;

namespace DragonSpark.Diagnostics.Logging.Enrichment
{
	sealed class CorrelationIdEnricher : Select<LoggerEnrichmentConfiguration, LoggerConfiguration>,
	                                     ILoggingEnrichmentConfiguration
	{
		public static CorrelationIdEnricher Default { get; } = new CorrelationIdEnricher();

		CorrelationIdEnricher() : base(x => x.WithCorrelationId()) {}
	}
}