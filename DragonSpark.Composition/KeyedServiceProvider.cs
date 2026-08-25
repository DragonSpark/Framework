using LightInject;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Composition;

sealed class KeyedServiceProvider : IKeyedServiceProvider
{
	readonly IServiceContainer _container;
	readonly IServiceProvider  _provider;

	public KeyedServiceProvider(IServiceContainer container, IServiceProvider provider)
	{
		_container = container;
		_provider  = provider;
	}

	public object? GetKeyedService(Type serviceType, object? serviceKey)
		=> _container.GetInstance(serviceType, serviceKey?.ToString() ?? string.Empty);

	public object GetRequiredKeyedService(Type serviceType, object? serviceKey)
		=> GetKeyedService(serviceType, serviceKey) ??
		   throw new
			   InvalidOperationException($"Unable to resolve keyed service '{serviceType}' with key '{serviceKey}'.");

	public object? GetService(Type serviceType) => _provider.GetService(serviceType);
}