using System;
using DragonSpark.Diagnostics.OpenTelemetry;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Sequences;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DragonSpark.Application.Mobile.Uno.Diagnostics;

sealed class ConfigureOpenTelemetry(Action<ILoggingBuilder> logging, Action<IServiceCollection> services)
	: ICommand<IHostBuilder>
{
	public ConfigureOpenTelemetry(Array<string> sources)
		: this(DefaultOpenTelemetryLoggingConfiguration.Default.Execute,
		       new DefaultOpenTelemetryConfiguration(sources).Execute) {}

	public void Execute(IHostBuilder parameter)
	{
		parameter.ConfigureLogging(logging).ConfigureServices(services);
	}
}
