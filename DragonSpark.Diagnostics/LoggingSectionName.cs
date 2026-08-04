namespace DragonSpark.Diagnostics;

sealed class LoggingSectionName : Text.Text
{
	public static LoggingSectionName Default { get; } = new();

	LoggingSectionName() : base("Logging") {}
}