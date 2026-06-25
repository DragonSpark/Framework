using System;
using DragonSpark.Compose;
using DragonSpark.Model.Results;
using LightInject;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Composition.Construction;

sealed class AssignableFactory : IAssignableFactory
{
    readonly IMutable<IServiceProviderFactory<IServiceContainer>?> _store;

    public AssignableFactory(IServiceProviderFactory<IServiceContainer> start)
        : this(new Variable<IServiceProviderFactory<IServiceContainer>>(start)) {}

    public AssignableFactory(IMutable<IServiceProviderFactory<IServiceContainer>?> store) => _store = store;

    public IServiceProviderFactory<IServiceContainer> Get() => _store.Get().Verify();

    public void Execute(IServiceProviderFactory<IServiceContainer> parameter)
    {
        _store.Execute(parameter);
    }

    public IServiceContainer CreateBuilder(IServiceCollection services)
        => _store.Get().Verify().CreateBuilder(services);

    public IServiceProvider CreateServiceProvider(IServiceContainer containerBuilder)
        => _store.Get().Verify().CreateServiceProvider(containerBuilder);
}