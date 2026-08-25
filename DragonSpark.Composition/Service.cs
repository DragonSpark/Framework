using DragonSpark.Composition.Construction;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using LightInject;
using LightInject.Microsoft.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Composition;

sealed class Service<T> : ISelect<IServiceCollection, T> where T : notnull
{
    public static Service<T> Default { get; } = new();

    Service() : this(NewDefaultContainer.Default) {}

    readonly IResult<ServiceContainer> _services;

    public Service(IResult<ServiceContainer> services) => _services = services;

    public T Get(IServiceCollection parameter)
    {
        var       container = _services.Get();
        using var provider  = container.CreateServiceProvider(parameter).CreateScope();
        var       next      = new ActivationAwareServiceProvider(container, provider.ServiceProvider);
        var       services  = new LocateAwareServiceProvider(next, parameter);
        var       result    = services.GetRequiredService<T>();
        return result;
    }
}