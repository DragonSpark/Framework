﻿using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;

namespace DragonSpark.Application.Compose;

sealed class InitializedAwareHostBuilder : IHostBuilder
{
	readonly IHostBuilder _previous;
	readonly ICommand     _initialized;

	public InitializedAwareHostBuilder(IHostBuilder previous, ICommand initialized)
		: this(previous, initialized, previous.Properties) {}

	public InitializedAwareHostBuilder(IHostBuilder previous, ICommand initialized,
	                                   IDictionary<object, object> properties)
	{
		Properties   = properties;
		_previous    = previous;
		_initialized = initialized;
	}

	public IHost Build()
	{
		var result = _previous.Build();
		_initialized.Execute();
		return result;
	}

	public IHostBuilder ConfigureAppConfiguration(Action<HostBuilderContext, IConfigurationBuilder> configureDelegate)
		=> _previous.ConfigureAppConfiguration(configureDelegate);

	public IHostBuilder ConfigureContainer<TContainerBuilder>(
		Action<HostBuilderContext, TContainerBuilder> configureDelegate)
		=> _previous.ConfigureContainer(configureDelegate);

	public IHostBuilder ConfigureHostConfiguration(Action<IConfigurationBuilder> configureDelegate)
		=> _previous.ConfigureHostConfiguration(configureDelegate);

	public IHostBuilder ConfigureServices(Action<HostBuilderContext, IServiceCollection> configureDelegate)
		=> _previous.ConfigureServices(configureDelegate);

	public IHostBuilder UseServiceProviderFactory<TContainerBuilder>(
		IServiceProviderFactory<TContainerBuilder> factory) where TContainerBuilder : notnull
		=> _previous.UseServiceProviderFactory(factory);

	public IHostBuilder UseServiceProviderFactory<TContainerBuilder>(
		Func<HostBuilderContext, IServiceProviderFactory<TContainerBuilder>> factory)
		where TContainerBuilder : notnull
		=> _previous.UseServiceProviderFactory(factory);

	public IDictionary<object, object> Properties { get; }
}