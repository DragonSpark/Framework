using Serilog;
using Serilog.Configuration;
using DragonSpark.Model.Selection;

namespace DragonSpark.Diagnostics.Logging
{
	sealed class LoggerEnrichmentSelector : Select<LoggerConfiguration, LoggerEnrichmentConfiguration>
	{
		public static LoggerEnrichmentSelector Default { get; } = new LoggerEnrichmentSelector();

		LoggerEnrichmentSelector() : base(x => x.Enrich) {}
	}
}