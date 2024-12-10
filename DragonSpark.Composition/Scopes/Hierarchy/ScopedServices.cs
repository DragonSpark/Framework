using System;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Composition.Scopes.Hierarchy;

sealed class ScopedServices : IScopedServices
{
    readonly IServiceScopeFactory _factory;
    readonly IServiceProvider _parent;

    public ScopedServices(IServiceScopeFactory factory, IServiceProvider parent)
    {
        _factory = factory;
        _parent = parent;
    }

    [MustDisposeResource]
    public IScopedServiceProvider Get()
    {
        var result = new ScopedServiceProvider(_factory.CreateAsyncScope());
        result.GetRequiredService<ParentScopeProvider>().Execute(_parent);
        return result;
    }
}
