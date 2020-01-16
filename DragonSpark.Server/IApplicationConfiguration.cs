using DragonSpark.Composition.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Runtime.Environment;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Server
{
	sealed class ConfigureFromEnvironment : SelectedCommand<IApplicationBuilder>
	{
		public static ConfigureFromEnvironment Default { get; } = new ConfigureFromEnvironment();

		ConfigureFromEnvironment() : base(ServiceConfigurationLocator.Default.Get) {}
	}

	sealed class ServiceConfigurationLocator : LocateComponent<IApplicationBuilder, IApplicationConfiguration>
	{
		public static ServiceConfigurationLocator Default { get; } = new ServiceConfigurationLocator();

		ServiceConfigurationLocator()
			: base(x => x.ApplicationServices.GetRequiredService<IComponentType>()) {}
	}

	public interface IApplicationConfiguration : ICommand<IApplicationBuilder> {}


}