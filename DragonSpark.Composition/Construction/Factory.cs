using LightInject;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Composition.Construction;

sealed class Factory : IServiceProviderFactory<IServiceContainer>
{
	readonly IServiceProviderFactory<IServiceContainer>                  _factory;
	readonly Func<IServiceContainer, IServiceProvider, IServiceProvider> _provider;

	public Factory(IServiceProviderFactory<IServiceContainer> factory)
		: this(factory, (container, provider)
			                => new ActivationAwareServiceProvider(provider as IKeyedServiceProvider ??
			                                                      new KeyedServiceProvider(container, provider))) {}

	public Factory(IServiceProviderFactory<IServiceContainer> factory,
	               Func<IServiceContainer, IServiceProvider, IServiceProvider> provider)
	{
		_factory  = factory;
		_provider = provider;
	}

	public IServiceContainer CreateBuilder(IServiceCollection services) => _factory.CreateBuilder(services);

	public IServiceProvider CreateServiceProvider(IServiceContainer containerBuilder)
	{
		var services = (IKeyedServiceProvider)_factory.CreateServiceProvider(containerBuilder);
		var result   = new ActivationAwareServiceProvider(services);
		containerBuilder.Decorate<IServiceProvider>((_, provider) => _provider(containerBuilder, provider));
		containerBuilder.Decorate<IServiceScopeFactory>((_, factory) => new ServiceScopeFactory(factory));
		return result;
	}
}