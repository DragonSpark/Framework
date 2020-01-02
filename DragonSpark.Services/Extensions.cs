using DragonSpark.Compose;
using DragonSpark.Composition.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Services.Application;
using Microsoft.AspNetCore.Hosting;

namespace DragonSpark.Services
{
	public static class Extensions
	{
		public static BuildHostContext WithConfiguration(this BuildHostContext @this)
			=> @this.WithConfiguration(StartupConfiguration.Default);

		public static BuildHostContext WithConfiguration<T>(this BuildHostContext @this) where T : class
			=> @this.WithConfiguration(StartupConfiguration<T>.Default);

		public static BuildHostContext WithConfiguration(this BuildHostContext @this,
		                                                 ICommand<IWebHostBuilder> configuration)
			=> @this.Get()
			        .Then()
			        .Select(new WebHostConfiguration(configuration.Execute))
			        .Out()
			        .To(Start.An.Extent<BuildHostContext>());
	}
}