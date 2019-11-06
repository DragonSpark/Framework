using Serilog;
using DragonSpark.Model.Selection.Alterations;

namespace DragonSpark.Diagnostics.Logging.Configuration
{
	public class LoggerConfigurations : CompositeAlteration<LoggerConfiguration>, ILoggingConfiguration
	{
		public LoggerConfigurations(params IAlteration<LoggerConfiguration>[] alterations) : base(alterations) {}
	}
}