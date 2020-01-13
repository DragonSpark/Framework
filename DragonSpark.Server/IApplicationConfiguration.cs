using DragonSpark.Composition.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using DragonSpark.Runtime.Environment;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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

	sealed class ServerHostBuilder<T> : ISelect<Array<string>, IHost> where T : class
	{
		public static ServerHostBuilder<T> Default { get; } = new ServerHostBuilder<T>();

		ServerHostBuilder() {}

		public IHost Get(Array<string> parameter) => Host.CreateDefaultBuilder(parameter)
		                                                 .ConfigureWebHostDefaults(builder => builder.UseStartup<T>())
		                                                 .Build();
	}
}