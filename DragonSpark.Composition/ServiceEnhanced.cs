using DragonSpark.Composition.Construction;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using LightInject;
using LightInject.Microsoft.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Composition;

sealed class ServiceEnhanced<T> : ISelect<IServiceCollection, T> where T : notnull
{
    public static ServiceEnhanced<T> Default { get; } = new();

    ServiceEnhanced() : this(NewDefaultContainer.Default) {}

    readonly IResult<ServiceContainer> _services;

    public ServiceEnhanced(IResult<ServiceContainer> services) => _services = services;

    public T Get(IServiceCollection parameter)
    {
        var       container = _services.Get();
        using var provider  = container.CreateServiceProvider(parameter).CreateScope();
        var       next      = new ActivationAwareServiceProvider(provider.ServiceProvider);
        var       services  = new LocateAwareServiceProvider(next, parameter);
        var       result    = services.GetRequiredService<T>();
        return result;
    }
}