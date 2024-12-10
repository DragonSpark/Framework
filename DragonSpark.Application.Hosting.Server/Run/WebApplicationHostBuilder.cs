using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DragonSpark.Application.Hosting.Server.Run;

sealed class WebApplicationHostBuilder : IHostBuilder, ISupportsConfigureWebHost
{
	readonly WebApplicationBuilder            _application;
	readonly IHostBuilder                     _host;
	readonly HashSet<Action<IWebHostBuilder>> _configurations;

	public WebApplicationHostBuilder(WebApplicationBuilder application)
		: this(application, new HashSet<Action<IWebHostBuilder>>()) {}

	public WebApplicationHostBuilder(WebApplicationBuilder application, HashSet<Action<IWebHostBuilder>> configurations)
		: this(application, application.Host, configurations) {}

	public WebApplicationHostBuilder(WebApplicationBuilder application, IHostBuilder host,
	                                 HashSet<Action<IWebHostBuilder>> configurations)
	{
		_application    = application;
		_host           = host;
		_configurations = configurations;
	}

	[MustDisposeResource]
	public IHost Build()
	{
		var result    = _application.Build();
		var configure = new WebHostBuilder(result);
		foreach (var action in _configurations)
		{
			action(configure);
		}

		return result;
	}

	public IHostBuilder ConfigureAppConfiguration(Action<HostBuilderContext, IConfigurationBuilder> configureDelegate)
		=> Next(_host.ConfigureAppConfiguration(configureDelegate));

	public IHostBuilder ConfigureContainer<TContainerBuilder>(
		Action<HostBuilderContext, TContainerBuilder> configureDelegate)
		=> Next(_host.ConfigureContainer(configureDelegate));

	public IHostBuilder ConfigureHostConfiguration(Action<IConfigurationBuilder> configureDelegate)
		=> Next(_host.ConfigureHostConfiguration(configureDelegate));

	public IHostBuilder ConfigureServices(Action<HostBuilderContext, IServiceCollection> configureDelegate)
		=> Next(_host.ConfigureServices(configureDelegate));

	public IHostBuilder UseServiceProviderFactory<TContainerBuilder>(IServiceProviderFactory<TContainerBuilder> factory)
		where TContainerBuilder : notnull
		=> Next(_host.UseServiceProviderFactory(factory));

	WebApplicationHostBuilder Next(IHostBuilder parameter) => new(_application, parameter, _configurations);

	public IHostBuilder UseServiceProviderFactory<TContainerBuilder>(
		Func<HostBuilderContext, IServiceProviderFactory<TContainerBuilder>> factory) where TContainerBuilder : notnull
		=> Next(_host.UseServiceProviderFactory(factory));

	public IHostBuilder ConfigureWebHost(Action<IWebHostBuilder> configure,
	                                     Action<WebHostBuilderOptions> configureOptions)
	{
		_configurations.Add(configure);
		return this;
	}

	public IDictionary<object, object> Properties => _host.Properties;
}
