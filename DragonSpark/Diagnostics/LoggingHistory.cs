using DragonSpark.Sources.Scopes;

namespace DragonSpark.Diagnostics
{
	public sealed class LoggingHistory : SingletonScope<LoggerHistorySink>
	{
		public static LoggingHistory Default { get; } = new LoggingHistory();
		LoggingHistory() : base( () => new LoggerHistorySink() ) {}
	}
}