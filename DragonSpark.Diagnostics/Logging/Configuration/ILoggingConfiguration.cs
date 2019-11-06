using Serilog;
using DragonSpark.Model.Selection.Alterations;

namespace DragonSpark.Diagnostics.Logging.Configuration
{
	public interface ILoggingConfiguration : IAlteration<LoggerConfiguration> {}
}