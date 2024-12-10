using System;
using System.Linq;
using DragonSpark.Model.Selection;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using Serilog.Extensions.Logging;
using ILogger = Serilog.ILogger;

namespace DragonSpark.Diagnostics;

sealed class CreateLoggingProvider : ISelect<IServiceProvider, ILoggerProvider>
{
    public static CreateLoggingProvider Default { get; } = new();

    CreateLoggingProvider() { }

    [MustDisposeResource]
    public ILoggerProvider Get(IServiceProvider parameter)
    {
        var enrichers = parameter.GetServices<ILogEventEnricher>().ToArray();
        parameter.GetRequiredService<LoggerConfiguration>().Enrich.With(enrichers);
        var logger = parameter.GetRequiredService<ILogger>();
        return new SerilogLoggerProvider(logger, true);
    }
}
