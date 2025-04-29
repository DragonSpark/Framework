using System;
using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Hosting.Maui.Run;

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