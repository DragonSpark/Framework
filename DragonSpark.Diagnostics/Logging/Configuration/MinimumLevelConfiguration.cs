using Serilog;
using Serilog.Events;

namespace DragonSpark.Diagnostics.Logging.Configuration
{
	sealed class MinimumLevelConfiguration : ILoggingConfiguration
	{
		public static MinimumLevelConfiguration Default { get; } = new MinimumLevelConfiguration();

		MinimumLevelConfiguration() : this(DefaultLoggingLevel.Default) {}

		readonly LogEventLevel _minimum;

		public MinimumLevelConfiguration(LogEventLevel minimum) => _minimum = minimum;

		public LoggerConfiguration Get(LoggerConfiguration parameter) => parameter.MinimumLevel.Is(_minimum);
	}
}