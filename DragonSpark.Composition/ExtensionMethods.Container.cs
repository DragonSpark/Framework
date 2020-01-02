using LightInject;
using LightInject.Microsoft.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DragonSpark.Composition
{
	// ReSharper disable once MismatchedFileName
	public static class ExtensionMethods
	{
		public static IHostBuilder UseLightInject(this IHostBuilder @this)
			=> @this.UseLightInject(ContainerOptions.Default.Clone().WithMicrosoftSettings().WithoutVariance());

		public static IHostBuilder UseLightInject(this IHostBuilder @this, ContainerOptions options)
			=> @this.UseLightInject(new LightInjectServiceProviderFactory(options));

		public static IHostBuilder UseLightInject(this IHostBuilder @this, IServiceContainer serviceContainer)
			=> @this.UseLightInject(new LightInjectServiceProviderFactory(serviceContainer));

		public static IHostBuilder UseLightInject(this IHostBuilder @this,
		                                          IServiceProviderFactory<IServiceContainer> factory)
			=> @this.UseServiceProviderFactory(factory);

		static ContainerOptions WithoutVariance(this ContainerOptions @this)
		{
			@this.EnableVariance = false;
			return @this;
		}
	}
}
