namespace DragonSpark.Diagnostics;

sealed class LoggingLevelPath : Text.Text
{
	public static LoggingLevelPath Default { get; } = new();

	LoggingLevelPath() : this(LoggingSectionName.Default) {}

	public LoggingLevelPath(string section) : base($"{section}:LogLevel") {}
}