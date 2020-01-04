using DragonSpark.Application;
using DragonSpark.Compose;
using DragonSpark.Composition.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using DragonSpark.Services.Application;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Services
{
	public static class Extensions
	{
		public static ISelect<Array<string>, Task> ConfiguredBy<T>(this IProgram @this) where T : class
			=> ServerHostBuilder<T>.Default.Select(@this.Get);

		/*public static BuildHostContext WithServer(this BuildHostContext @this)
			=> @this.WithServer(ConfigureFromEnvironment.Default);*/

		/*public static BuildHostContext WithServer<T>(this BuildHostContext @this) where T : class, IConfigurator
			=> @this.WithServer(StartupConfiguration<T>.Default);
		*/

		public static BuildHostContext Apply(this BuildHostContext @this, IServerProfile profile)
			=> @this.WithServer(new ServerConfiguration(profile.Execute).Then(ApplyStartupConfiguration.Default))
			        .Configure(profile);

		public static IServerProfile Then(this IServerProfile @this, ICommand<IServiceCollection> other)
			=> new ServerProfile(A.Command<IServiceCollection>(@this).Then(other), @this.Execute);

		public static IServerProfile Then(this IServerProfile @this, ICommand<IApplicationBuilder> other)
			=> new ServerProfile(@this.Execute, A.Command<IApplicationBuilder>(@this).Then(other));

		public static IServerProfile WithEnvironmentalConfiguration(this IServerProfile @this)
			=> new ServerProfile(A.Command<IServiceCollection>(@this)
			                      .Then(Composition.Compose.ConfigureFromEnvironment.Default),
			                     A.Command<IApplicationBuilder>(@this)
			                      .WithEnvironmentalConfiguration()
			                      .Execute);

		public static ICommand<IApplicationBuilder> WithEnvironmentalConfiguration(
			this ICommand<IApplicationBuilder> @this) => @this.Then(ConfigureFromEnvironment.Default).Get();

		/*public static BuildHostContext WithServer(this BuildHostContext @this, ICommand<IApplicationBuilder> configure)
			=> @this.WithServer(configure.Execute);

		public static BuildHostContext WithServer(this BuildHostContext @this,
		                                          Action<IApplicationBuilder> configuration)
			=> @this.WithServer(new ServerConfiguration(configuration).Then(ApplyStartupConfiguration.Default));

		public static BuildHostContext WithServer(this BuildHostContext @this, ICommand<IWebHostBuilder> configuration)
			=> @this.WithServer(configuration.Execute);*/

		public static BuildHostContext WithServer(this BuildHostContext @this, Action<IWebHostBuilder> configuration)
			=> @this.Select(new WebHostConfiguration(configuration));
	}
}