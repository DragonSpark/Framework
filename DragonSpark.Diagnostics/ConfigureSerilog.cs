using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace DragonSpark.Diagnostics;

sealed class ConfigureSerilog : ICommand<IServiceCollection>
{
	readonly Func<IServiceProvider, ILoggerProvider> _provider;

	public ConfigureSerilog(Func<IServiceProvider, ILoggerProvider> provider) => _provider = provider;

	public void Execute(IServiceCollection parameter)
	{
		parameter.AddScoped(_provider);
	}
}