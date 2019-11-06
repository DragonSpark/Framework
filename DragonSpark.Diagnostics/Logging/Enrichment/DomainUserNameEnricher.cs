using Serilog;
using Serilog.Configuration;
using DragonSpark.Diagnostics.Logging.Configuration;
using DragonSpark.Model.Selection;

namespace DragonSpark.Diagnostics.Logging.Enrichment
{
	sealed class DomainUserNameEnricher : Select<LoggerEnrichmentConfiguration, LoggerConfiguration>,
	                                      ILoggingEnrichmentConfiguration
	{
		public static DomainUserNameEnricher Default { get; } = new DomainUserNameEnricher();

		DomainUserNameEnricher() : base(x => x.WithEnvironmentUserName()) {}
	}
}