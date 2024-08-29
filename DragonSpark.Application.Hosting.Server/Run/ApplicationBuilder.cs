using DragonSpark.Application.Run;
using DragonSpark.Model.Selection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.Metrics;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace DragonSpark.Application.Hosting.Server.Run;

// TODO

sealed class InitializeBuilder : ISelect<string[], ApplicationBuilder>
{
	public static InitializeBuilder Default { get; } = new();

	InitializeBuilder() {}

	public ApplicationBuilder Get(string[] parameter)
	{
		var result = new ApplicationBuilder(parameter);
		result.Services.AddSingleton(result);
		return result;
	}
}

sealed class ApplicationBuilder : IHostedApplicationBuilder
{
	readonly WebApplicationBuilder _previous;

	public ApplicationBuilder(string[] arguments) : this(WebApplication.CreateBuilder(arguments)) {}

	public ApplicationBuilder(WebApplicationBuilder previous)
		: this(previous, new WebApplicationHostBuilder(previous)) {}

	public ApplicationBuilder(WebApplicationBuilder previous, IHostBuilder builder)
		: this(previous, builder, builder.Properties) {}

	public ApplicationBuilder(WebApplicationBuilder previous, IHostBuilder builder,
	                          IDictionary<object, object> properties)
	{
		_previous  = previous;
		Builder    = builder;
		Properties = properties;
	}

	public IHostedApplicationBuilder With(IHostBuilder builder)
		=> new ApplicationBuilder(_previous, builder, Properties);

	public void ConfigureContainer<TContainerBuilder>(IServiceProviderFactory<TContainerBuilder> factory,
	                                                  Action<TContainerBuilder>? configure = null)
		where TContainerBuilder : notnull
	{
		((IHostApplicationBuilder)_previous).ConfigureContainer(factory, configure);
	}

	public IConfigurationManager Configuration => _previous.Configuration;

	public IHostEnvironment Environment => _previous.Environment;

	public ILoggingBuilder Logging => _previous.Logging;

	public IMetricsBuilder Metrics => _previous.Metrics;

	public IHostBuilder Builder { get; }
	public IDictionary<object, object> Properties { get; }
	public IServiceCollection Services => _previous.Services;
}