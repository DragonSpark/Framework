using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Composition.Scopes.Hierarchy;

[MustDisposeResource]
sealed class ScopedServiceProvider : ServiceProvider, IScopedServiceProvider
{
    readonly AsyncServiceScope _scope;

    public ScopedServiceProvider(AsyncServiceScope scope) : this(scope, scope.ServiceProvider) { }

    public ScopedServiceProvider(AsyncServiceScope scope, IServiceProvider provider) : base(provider) => _scope = scope;

    public void Dispose()
    {
        _scope.Dispose();
    }

    public ValueTask DisposeAsync() => _scope.DisposeAsync();
}
