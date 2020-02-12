using DragonSpark.Composition.Compose;
using DragonSpark.Runtime.Environment;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application
{
	sealed class ServiceConfigurationLocator : LocateComponent<IApplicationBuilder, IApplicationConfiguration>
	{
		public static ServiceConfigurationLocator Default { get; } = new ServiceConfigurationLocator();

		ServiceConfigurationLocator()
			: base(x => x.ApplicationServices.GetRequiredService<IComponentType>()) {}
	}
}