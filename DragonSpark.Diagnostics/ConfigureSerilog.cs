using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using ILogger = Serilog.ILogger;

namespace DragonSpark.Diagnostics;

sealed class ConfigureSerilog : ICommand<IHostBuilder>
{
	readonly Action<HostBuilderContext, ILoggingBuilder> _add;

	public ConfigureSerilog(Func<IConfiguration, ILogger> logger) : this(new AddProvider(logger).Execute) {}

	public ConfigureSerilog(Action<HostBuilderContext, ILoggingBuilder> add) => _add = add;

	public void Execute(IHostBuilder parameter)
	{
		parameter.ConfigureLogging(_add);
	}
}