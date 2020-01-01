using LightInject;
using LightInject.Microsoft.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DragonSpark.Composition
{
	public static partial class Extensions
	{
		sealed class Registration : IServiceConfiguration
		{
			readonly IServiceProviderFactory<IServiceContainer> _factory;

			public Registration(IServiceProviderFactory<IServiceContainer> factory) => _factory = factory;

			public void Execute(IServiceCollection parameter)
			{
				parameter.AddSingleton(_factory);
			}
		}

		static ContainerOptions WithoutVariance(this ContainerOptions @this)
		{
			@this.EnableVariance = false;
			return @this;
		}

		public static IHostBuilder UseLightInject(this IHostBuilder @this)
			=> @this.UseLightInject(ContainerOptions.Default.Clone().WithMicrosoftSettings().WithoutVariance());

		public static IHostBuilder UseLightInject(this IHostBuilder @this, ContainerOptions options)
			=> @this.UseLightInject(new LightInjectServiceProviderFactory(options));

		public static IHostBuilder UseLightInject(this IHostBuilder @this, IServiceContainer serviceContainer)
			=> @this.UseLightInject(new LightInjectServiceProviderFactory(serviceContainer));

		public static IHostBuilder UseLightInject(this IHostBuilder @this,
		                                          IServiceProviderFactory<IServiceContainer> factory)
			=> @this.ConfigureServices(new Registration(factory).Execute);
	}
}