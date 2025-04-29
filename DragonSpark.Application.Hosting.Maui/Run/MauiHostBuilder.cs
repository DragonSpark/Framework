using System;
using System.Collections.Generic;
using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Model.Results;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Maui.Hosting;

namespace DragonSpark.Application.Hosting.Maui.Run;

sealed class MauiHostBuilder : IHostBuilder
{
    readonly MauiAppBuilder    _builder;
    readonly IMutable<object?> _factory;

    public MauiHostBuilder(MauiAppBuilder builder)
        : this(builder, Host.CreateDefaultBuilder(), new Variable<object>()) {}

    public MauiHostBuilder(MauiAppBuilder builder, IHostBuilder host, IMutable<object?> factory)
    {
        _builder   = builder;
        _factory   = factory;
        Properties = host.Properties;

        var result = host.Build();
        _builder.Services.AddSingleton(result.Services.GetRequiredService<HostBuilderContext>());
    }

    public IHost Build()
    {
        throw new InvalidOperationException();
    }

    public IHostBuilder ConfigureAppConfiguration(Action<HostBuilderContext, IConfigurationBuilder> configureDelegate)
    {
        configureDelegate(New(), _builder.Configuration);
        return this;
    }

#pragma warning disable CS8633 // Nullability in constraints for type parameter doesn't match the constraints for type parameter in implicitly implemented interface method'.
    public IHostBuilder ConfigureContainer<TContainerBuilder>(
        Action<HostBuilderContext, TContainerBuilder> configureDelegate) where TContainerBuilder : notnull
    {
        var factory   = _factory.Get().Verify().To<IServiceProviderFactory<TContainerBuilder>>();
        var container = factory.CreateBuilder(_builder.Services);
        configureDelegate(New(), container);
        return this;
    }
#pragma warning restore CS8633 // Nullability in constraints for type parameter doesn't match the constraints for type parameter in implicitly implemented interface method'.

    HostBuilderContext New() => _builder.Services.GetRequiredInstance<HostBuilderContext>();

    public IHostBuilder ConfigureHostConfiguration(Action<IConfigurationBuilder> configureDelegate)
    {
        configureDelegate(_builder.Configuration);
        return this;
    }

    public IHostBuilder ConfigureServices(Action<HostBuilderContext, IServiceCollection> configureDelegate)
    {
        configureDelegate(New(), _builder.Services);
        return this;
    }

    public IHostBuilder UseServiceProviderFactory<TContainerBuilder>(IServiceProviderFactory<TContainerBuilder> factory)
        where TContainerBuilder : notnull
    {
        Assign(factory);
        return this;
    }

    public IHostBuilder UseServiceProviderFactory<TContainerBuilder>(
        Func<HostBuilderContext, IServiceProviderFactory<TContainerBuilder>> factory) where TContainerBuilder : notnull
    {
        var previous = factory(New());
        Assign(previous);
        return this;
    }

    void Assign<TContainerBuilder>(IServiceProviderFactory<TContainerBuilder> parameter)
        where TContainerBuilder : notnull
    {
        var adapter = new Adapter<TContainerBuilder>(parameter);
        _factory.Execute(adapter);
        _builder.ConfigureContainer(adapter);
    }

    public IDictionary<object, object> Properties { get; }
}