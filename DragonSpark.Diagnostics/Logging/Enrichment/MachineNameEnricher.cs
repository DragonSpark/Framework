using Serilog;
using Serilog.Configuration;
using DragonSpark.Diagnostics.Logging.Configuration;
using DragonSpark.Model.Selection;

namespace DragonSpark.Diagnostics.Logging.Enrichment
{
	sealed class MachineNameEnricher : Select<LoggerEnrichmentConfiguration, LoggerConfiguration>, ILoggingEnrichmentConfiguration
	{
		public static MachineNameEnricher Default { get; } = new MachineNameEnricher();

		MachineNameEnricher() : base(ContextLoggerConfigurationExtension.WithMachineName) {}
	}
}