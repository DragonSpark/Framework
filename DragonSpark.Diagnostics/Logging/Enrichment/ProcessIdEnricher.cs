using Serilog;
using Serilog.Configuration;
using DragonSpark.Diagnostics.Logging.Configuration;
using DragonSpark.Model.Selection;

namespace DragonSpark.Diagnostics.Logging.Enrichment
{
	sealed class ProcessIdEnricher : Select<LoggerEnrichmentConfiguration, LoggerConfiguration>, ILoggingEnrichmentConfiguration
	{
		public static ProcessIdEnricher Default { get; } = new ProcessIdEnricher();

		ProcessIdEnricher() : base(x => x.WithProcessId()) {}
	}
}