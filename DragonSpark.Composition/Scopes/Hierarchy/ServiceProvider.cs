using System;

namespace DragonSpark.Composition.Scopes.Hierarchy;

class ServiceProvider : IParentServiceProvider
{
	readonly IServiceProvider _provider;

	protected ServiceProvider(IServiceProvider provider) => _provider = provider;

	public object? GetService(Type serviceType) => _provider.GetService(serviceType);
}