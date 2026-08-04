using DragonSpark.Model.Selection;
using Microsoft.Extensions.Logging;
using Serilog.Events;

namespace DragonSpark.Diagnostics;

sealed class LogEventLevels : ISelect<LogLevel, LogEventLevel>
{
	public static LogEventLevels Default { get; } = new();

	LogEventLevels() {}

	public LogEventLevel Get(LogLevel parameter) => parameter switch
	{
		LogLevel.Trace => LogEventLevel.Verbose,
		LogLevel.Debug => LogEventLevel.Debug,
		LogLevel.Information => LogEventLevel.Information,
		LogLevel.Warning => LogEventLevel.Warning,
		LogLevel.Error => LogEventLevel.Error,
		LogLevel.Critical => LogEventLevel.Fatal,
		LogLevel.None => (LogEventLevel)int.MaxValue,
		_ => LogEventLevel.Information
	};
}