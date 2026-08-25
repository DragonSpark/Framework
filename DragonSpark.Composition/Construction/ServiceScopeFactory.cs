using JetBrains.Annotations;
using LightInject;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Composition.Construction;

sealed class ServiceScopeFactory : IServiceScopeFactory
{
	readonly IServiceContainer    _container;
	readonly IServiceScopeFactory _factory;

	public ServiceScopeFactory(IServiceContainer container, IServiceScopeFactory factory)
	{
		_factory   = factory;
		_container = container;
	}

	[MustDisposeResource]
	public IServiceScope CreateScope() => new Scope(_container, _factory.CreateAsyncScope());

	[MustDisposeResource]
	sealed class Scope : IServiceScope, IAsyncDisposable
	{
		readonly AsyncServiceScope _scope;

		public Scope(IServiceContainer container, AsyncServiceScope scope)
			: this(scope, new KeyedServiceProvider(container, scope.ServiceProvider)) {}

		public Scope(AsyncServiceScope scope, IKeyedServiceProvider provider)
			: this(scope, (IServiceProvider)new ActivationAwareServiceProvider(provider)) {}

		public Scope(AsyncServiceScope scope, IServiceProvider provider)
		{
			_scope          = scope;
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