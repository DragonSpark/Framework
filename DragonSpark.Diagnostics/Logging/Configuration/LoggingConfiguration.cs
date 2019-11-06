using DragonSpark.Runtime.Environment;

namespace DragonSpark.Diagnostics.Logging.Configuration
{
	sealed class LoggingConfiguration : Component<ILoggingConfiguration>
	{
		public static LoggingConfiguration Default { get; } = new LoggingConfiguration();

		LoggingConfiguration() : base(DefaultLoggingConfiguration.Default.Self) {}
	}
}