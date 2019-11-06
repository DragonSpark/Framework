using Serilog;
using Serilog.Configuration;
using DragonSpark.Diagnostics.Logging.Configuration;
using DragonSpark.Model.Selection;

namespace DragonSpark.Diagnostics.Logging.Enrichment
{
	sealed class MemoryEnricher : Select<LoggerEnrichmentConfiguration, LoggerConfiguration>, ILoggingEnrichmentConfiguration
	{
		public static MemoryEnricher Default { get; } = new MemoryEnricher();

		MemoryEnricher() : base(x => x.WithMemoryUsage()) {}
	}
}