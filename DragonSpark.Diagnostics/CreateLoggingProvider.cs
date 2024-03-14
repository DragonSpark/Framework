using DragonSpark.Model.Selection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using Serilog.Extensions.Logging;
using System;
using System.Linq;
using ILogger = Serilog.ILogger;

namespace DragonSpark.Diagnostics;

sealed class CreateLoggingProvider : ISelect<IServiceProvider, ILoggerProvider>
{
	public static CreateLoggingProvider Default { get; } = new();

	CreateLoggingProvider() {}

	public ILoggerProvider Get(IServiceProvider parameter)
	{
		var enrichers = parameter.GetServices<ILogEventEnricher>().ToArray();
		parameter.GetRequiredService<LoggerConfiguration>().Enrich.With(enrichers);
		var logger = parameter.GetRequiredService<ILogger>();
		return new SerilogLoggerProvider(logger, true);
	}
}