using Serilog;
using Serilog.Configuration;
using DragonSpark.Diagnostics.Logging.Configuration;
using DragonSpark.Model.Selection;

namespace DragonSpark.Diagnostics.Logging.Enrichment
{
	sealed class ExceptionHashEnricher : Select<LoggerEnrichmentConfiguration, LoggerConfiguration>,
	                                     ILoggingEnrichmentConfiguration
	{
		public static ExceptionHashEnricher Default { get; } = new ExceptionHashEnricher();

		ExceptionHashEnricher() : base(x => x.WithExceptionStackTraceHash()) {}
	}
}