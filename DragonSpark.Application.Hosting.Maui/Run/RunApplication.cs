using System;
using System.Collections.Generic;
using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using DragonSpark.Reflection.Members;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Maui.Hosting;

namespace DragonSpark.Application.Hosting.Maui.Run;

public abstract class RunApplication(
    Func<MauiAppBuilder, MauiAppBuilder> builder,
    Func<MauiAppBuilder, MauiApp> application)
    : Mobile.Maui.Run.RunApplication(builder, application)
{
    protected RunApplication(Func<IHostBuilder, IHostBuilder> select, Action<MauiAppBuilder> configure)
        : this(select, configure, x => x.Build()) {}

    protected RunApplication(Func<IHostBuilder, IHostBuilder> select, Action<MauiAppBuilder> configure,
                             Func<MauiAppBuilder, MauiApp> host)
        : this(new InitializeBuilder(select, configure).Get, host) {}
}

sealed class InitializeBuilder(Func<IHostBuilder, IHostBuilder> host, Action<MauiAppBuilder> configure)
    : ISelect<MauiAppBuilder, MauiAppBuilder>
{
    public MauiAppBuilder Get(MauiAppBuilder parameter)
    {
        host(new MauiHostBuilder(parameter));
        configure(parameter);
        return parameter;
    }
}

sealed class HostBuilderServices : FieldAccessor<HostBuilder, IServiceCollection>
{
    public static HostBuilderServices Default { get; } = new();

    HostBuilderServices() : base("_renderFragment") {}
}

// TODO
sealed class MauiHostBuilder : IHostBuilder
{
    readonly MauiAppBuilder    _builder;
    readonly IHostBuilder      _host;
    readonly IMutable<object?> _factory;

    public MauiHostBuilder(MauiAppBuilder builder)
        : this(builder, Host.CreateDefaultBuilder(), new Variable<object>()) {}

    public MauiHostBuilder(MauiAppBuilder builder, IHostBuilder host, IMutable<object?> factory)
    {
        _builder   = builder;
        _host      = host;
        _factory   = factory;
        Properties = host.Properties;

        // TODO
        var result = _host.Build();
        _builder.Services.AddSingleton(result.Services.GetRequiredService<HostBuilderContext>());
    }

    public IHost Build()
    {
        throw new InvalidOperationException();
        /*var result = _host.Build();
        // _builder.Services.AddSingleton(result.Services.GetRequiredService<HostBuilderContext>());
        return result;*/
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

sealed class Adapter<T> : Select<IServiceCollection, T>, IServiceProviderFactory<T> where T : notnull
{
    readonly IServiceProviderFactory<T> _previous;

    public Adapter(IServiceProviderFactory<T> previous)
        : this(previous, Start.A.Selection<IServiceCollection, T>(previous.CreateBuilder).Then().Stores().New()) {}

    public Adapter(IServiceProviderFactory<T> previous, ISelect<IServiceCollection, T> key)
        : base(key) => _previous = previous;

    public T CreateBuilder(IServiceCollection services) => Get(services);

    public IServiceProvider CreateServiceProvider(T containerBuilder)
        => _previous.CreateServiceProvider(containerBuilder);
}