using DragonSpark.Model.Selection;
using Serilog;
using ILogger = Serilog.ILogger;

namespace DragonSpark.Diagnostics;

sealed class CreateLogger : ISelect<LoggerConfiguration, ILogger>
{
	public static CreateLogger Default { get; } = new();

	CreateLogger() {}

	public ILogger Get(LoggerConfiguration parameter) => parameter.CreateLogger();
}