using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Composition.Scopes.Hierarchy;

sealed class ScopedServiceProvider : ServiceProvider, IScopedServiceProvider
{
	readonly AsyncServiceScope _scope;

	public ScopedServiceProvider(AsyncServiceScope scope) : this(scope, scope.ServiceProvider) {}

	public ScopedServiceProvider(AsyncServiceScope scope, IServiceProvider provider) : base(provider) => _scope = scope;

	public void Dispose()
	{
		_scope.Dispose();
	}

	public ValueTask DisposeAsync() => _scope.DisposeAsync();
}