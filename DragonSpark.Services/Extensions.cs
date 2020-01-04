using DragonSpark.Application;
using DragonSpark.Composition.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using DragonSpark.Services.Application;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;

namespace DragonSpark.Services
{
	public static class Extensions
	{
		public static ISelect<Array<string>, Task> ConfiguredBy<T>(this IProgram @this) where T : class, IConfigurator
			=> ServerHostBuilder<T>.Default.Select(@this.Get);

		public static BuildHostContext WithConfigurationFromEnvironment(this BuildHostContext @this)
			=> @this.WithConfiguration<EnvironmentalStartup>();

		public static BuildHostContext WithConfiguration(this BuildHostContext @this)
			=> @this.WithConfiguration(StartupConfiguration.Default);

		public static BuildHostContext WithConfiguration<T>(this BuildHostContext @this) where T : class
			=> @this.WithConfiguration(StartupConfiguration<T>.Default);

		public static BuildHostContext WithConfiguration(this BuildHostContext @this,
		                                                 ICommand<IWebHostBuilder> configuration)
			=> @this.Select(new WebHostConfiguration(configuration.Execute));
	}
}