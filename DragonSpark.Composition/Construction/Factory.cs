using LightInject;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Composition.Construction
{
	sealed class Factory : IServiceProviderFactory<IServiceContainer>
	{
		readonly IServiceProviderFactory<IServiceContainer> _factory;

		public Factory(IServiceProviderFactory<IServiceContainer> factory) => _factory = factory;

		public IServiceContainer CreateBuilder(IServiceCollection services) => _factory.CreateBuilder(services);

		public IServiceProvider CreateServiceProvider(IServiceContainer containerBuilder)
		{
			var result = new ActivationAwareServiceProvider(_factory.CreateServiceProvider(containerBuilder));
			containerBuilder.Decorate<IServiceProvider>((_, provider)
				                                            => new ActivationAwareServiceProvider(provider));
			containerBuilder.Decorate<IServiceScopeFactory>((_, factory) => new ServiceScopeFactory(factory));
			return result;
		}
	}
}