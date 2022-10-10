using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Composition.Construction;

sealed class ServiceScopeFactory : IServiceScopeFactory
{
	readonly IServiceScopeFactory _factory;

	public ServiceScopeFactory(IServiceScopeFactory factory) => _factory = factory;

	public IServiceScope CreateScope() => new Scope(_factory.CreateAsyncScope());

	sealed class Scope : IServiceScope, IAsyncDisposable
	{
		readonly AsyncServiceScope _scope;

		public Scope(AsyncServiceScope scope)
			: this(scope, new ActivationAwareServiceProvider(scope.ServiceProvider)) {}

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