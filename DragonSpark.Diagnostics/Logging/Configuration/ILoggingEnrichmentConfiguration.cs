using Serilog;
using Serilog.Configuration;
using DragonSpark.Model.Selection;

namespace DragonSpark.Diagnostics.Logging.Configuration
{
	public interface ILoggingEnrichmentConfiguration : ISelect<LoggerEnrichmentConfiguration, LoggerConfiguration> {}
}