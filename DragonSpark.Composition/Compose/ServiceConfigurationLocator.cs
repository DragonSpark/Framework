using DragonSpark.Runtime.Environment;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Composition.Compose
{
	sealed class ServiceConfigurationLocator : LocateComponent<IServiceCollection, IServiceConfiguration>
	{
		public static ServiceConfigurationLocator Default { get; } = new ServiceConfigurationLocator();

		ServiceConfigurationLocator() : base(x => x.GetRequiredInstance<IComponentType>()) {}
	}
}