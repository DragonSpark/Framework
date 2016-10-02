using DragonSpark.Sources;

namespace DragonSpark.Diagnostics
{
	public sealed class LoggingHistory : Scope<LoggerHistorySink>
	{
		public static LoggingHistory Default { get; } = new LoggingHistory();
		LoggingHistory() : base( Factory.GlobalCache( () => new LoggerHistorySink() ) ) {}
	}
}