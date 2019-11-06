using Serilog;
using Serilog.Configuration;
using DragonSpark.Diagnostics.Logging.Configuration;
using DragonSpark.Model.Selection;

namespace DragonSpark.Diagnostics.Logging.Enrichment
{
	sealed class UserNameEnricher : Select<LoggerEnrichmentConfiguration, LoggerConfiguration>,
	                                ILoggingEnrichmentConfiguration
	{
		public static UserNameEnricher Default { get; } = new UserNameEnricher();

		UserNameEnricher() : base(x => x.WithUserName()) {}
	}
}