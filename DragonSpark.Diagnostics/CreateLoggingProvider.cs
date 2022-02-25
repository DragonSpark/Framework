using DragonSpark.Model.Selection;
using Microsoft.Extensions.Logging;
using Serilog.Extensions.Logging;
using ILogger = Serilog.ILogger;

namespace DragonSpark.Diagnostics;

sealed class CreateLoggingProvider : ISelect<ILogger, ILoggerProvider>
{
	public static CreateLoggingProvider Default { get; } = new();

	CreateLoggingProvider() {}

	public ILoggerProvider Get(ILogger parameter) => new SerilogLoggerProvider(parameter, true);
}