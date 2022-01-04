using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;

namespace DragonSpark.Diagnostics;

sealed class ConfigureSerilog : ICommand<IServiceCollection>
{
	readonly Func<IServiceCollection, ILogger> _logger;

	public ConfigureSerilog(Func<IServiceCollection, ILogger> logger) => _logger = logger;

	public void Execute(IServiceCollection parameter)
	{
		var logger = _logger(parameter);
		parameter.AddLogging(new AddSerilog(logger).Execute);
	}
}