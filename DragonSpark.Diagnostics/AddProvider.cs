using DragonSpark.Model.Commands;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog.Extensions.Logging;
using System;
using ILogger = Serilog.ILogger;

namespace DragonSpark.Diagnostics;

sealed class AddProvider : ICommand<(HostBuilderContext Context, ILoggingBuilder Builder)>
{
	readonly Func<IConfiguration, ILogger> _logger;

	public AddProvider(Func<IConfiguration, ILogger> logger) => _logger = logger;

	public void Execute((HostBuilderContext Context, ILoggingBuilder Builder) parameter)
	{
		var (context, builder) = parameter;
		var logger = _logger(context.Configuration);
		builder.AddProvider(new SerilogLoggerProvider(logger, true));
	}
}