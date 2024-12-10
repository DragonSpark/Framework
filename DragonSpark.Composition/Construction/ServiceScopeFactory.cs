using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Composition.Construction;

sealed class ServiceScopeFactory : IServiceScopeFactory
{
    readonly IServiceScopeFactory _factory;

    public ServiceScopeFactory(IServiceScopeFactory factory) => _factory = factory;

    [MustDisposeResource]
    public IServiceScope CreateScope() => new Scope(_factory.CreateAsyncScope());

    [MustDisposeResource]
    sealed class Scope : IServiceScope, IAsyncDisposable
    {
        readonly AsyncServiceScope _scope;

        public Scope(AsyncServiceScope scope)
            : this(scope, new ActivationAwareServiceProvider(scope.ServiceProvider)) { }

        public Scope(AsyncServiceScope scope, IServiceProvider provider)
        {
            _scope = scope;
            ServiceProvider = provider;
        }

        public IServiceProvider ServiceProvider { get; }

        public void Dispose()
        {
            _scope.Dispose();
        }

        public ValueTask DisposeAsync() => _scope.DisposeAsync();
    }
}
