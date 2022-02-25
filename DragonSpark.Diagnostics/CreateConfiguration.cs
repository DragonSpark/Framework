using DragonSpark.Model.Selection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Core;
using System;
using System.Linq;

namespace DragonSpark.Diagnostics;

sealed class CreateConfiguration : ISelect<IServiceProvider, LoggerConfiguration>
{
	public static CreateConfiguration Default { get; } = new();

	CreateConfiguration() {}

	public LoggerConfiguration Get(IServiceProvider parameter)
	{
		var enrichers = parameter.GetServices<ILogEventEnricher>().ToArray();
		var result = new LoggerConfiguration().ReadFrom.Configuration(parameter.GetRequiredService<IConfiguration>())
		                                      .Enrich.With(enrichers);
		return result;
	}
}