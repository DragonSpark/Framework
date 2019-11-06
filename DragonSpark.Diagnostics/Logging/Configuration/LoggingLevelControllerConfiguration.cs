using Serilog;
using Serilog.Core;

namespace DragonSpark.Diagnostics.Logging.Configuration
{
	sealed class LoggingLevelControllerConfiguration : ILoggingConfiguration
	{
		public static LoggingLevelControllerConfiguration Default { get; } = new LoggingLevelControllerConfiguration();

		LoggingLevelControllerConfiguration() : this(LoggingLevelController.Default) {}

		readonly LoggingLevelSwitch _switch;

		public LoggingLevelControllerConfiguration(LoggingLevelSwitch @switch) => _switch = @switch;

		public LoggerConfiguration Get(LoggerConfiguration parameter) => parameter.MinimumLevel.ControlledBy(_switch);
	}
}