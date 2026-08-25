using LightInject;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Composition.Construction;

sealed class Factory : IServiceProviderFactory<IServiceContainer>
{
	readonly IServiceProviderFactory<IServiceContainer> _factory;

	public Factory(IServiceProviderFactory<IServiceContainer> factory) => _factory = factory;

	public IServiceContainer CreateBuilder(IServiceCollection services) => _factory.CreateBuilder(services);

	public IServiceProvider CreateServiceProvider(IServiceContainer containerBuilder)
	{
		var services = _factory.CreateServiceProvider(containerBuilder);
		var keyed    = new KeyedServiceProvider(containerBuilder, services);
		var result   = new ActivationAwareServiceProvider(keyed);
		containerBuilder.Decorate<IServiceProvider>((_, provider)
			                                            => new ActivationAwareServiceProvider(containerBuilder,
			                                                                                  provider));
		containerBuilder.Decorate<IServiceScopeFactory>((_, factory)
			                                                => new ServiceScopeFactory(containerBuilder, factory));
		return result;
	}
}