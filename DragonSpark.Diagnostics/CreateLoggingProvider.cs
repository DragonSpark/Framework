using DragonSpark.Model.Selection;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NetFabric.Hyperlinq;
using Serilog;
using Serilog.Core;
using Serilog.Extensions.Logging;
using System;
using System.Buffers;
using ILogger = Serilog.ILogger;

namespace DragonSpark.Diagnostics;

sealed class CreateLoggingProvider : ISelect<IServiceProvider, ILoggerProvider>
{
	public static CreateLoggingProvider Default { get; } = new();

	CreateLoggingProvider() {}

	[MustDisposeResource]
	public ILoggerProvider Get(IServiceProvider parameter)
	{
		using var enrichers = parameter.GetServices<ILogEventEnricher>()
		                               .AsValueEnumerable()
		                               .ToArray(ArrayPool<ILogEventEnricher>.Shared);
		if (enrichers.Length > 0)
		{
			parameter.GetRequiredService<LoggerConfiguration>().Enrich.With(enrichers.Rented);
		}

		var logger = parameter.GetRequiredService<ILogger>();
		return new SerilogLoggerProvider(logger, true);
	}
}