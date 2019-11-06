using Serilog;
using DragonSpark.Model.Selection.Alterations;

namespace DragonSpark.Diagnostics.Logging.Configuration
{
	class LoggingConfigurations : CompositeAlteration<LoggerConfiguration>, ILoggingConfiguration
	{
		public LoggingConfigurations(params IAlteration<LoggerConfiguration>[] alterations) : base(alterations) {}
	}
}