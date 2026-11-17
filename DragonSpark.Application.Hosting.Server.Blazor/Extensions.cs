using DragonSpark.Application.AspNet;
using DragonSpark.Application.AspNet.Compose;
using DragonSpark.Composition.Compose;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Builder;
using System.Reflection;

namespace DragonSpark.Application.Hosting.Server.Blazor;

public static class Extensions
{
	extension(BuildHostContext @this)
	{
		public ApplicationProfileContext WithBlazorServerApplication() => @this.Apply(BlazorApplicationProfile.Default);

		public ApplicationProfileContext WithBlazorServerApplication(Action<IApplicationBuilder> builder)
			=> @this.Apply(new BlazorApplicationProfile(builder));

		public ApplicationProfileContext WithBlazorServerApplication<T>(params Assembly[] additional)
			=> @this.WithBlazorServerApplication<T>(_ => {}, additional);

		public ApplicationProfileContext WithBlazorServerApplication<T>(Action<IApplicationBuilder> builder,
		                                                                params Assembly[] additional)
			=> @this.Apply(new BlazorApplicationProfile(additional.Apply<T>(builder)));

		public ApplicationProfileContext WithOptimizedBlazorServerApplication(
			Action<IApplicationBuilder> configure, byte receive = 32)
		{
			var configuration = new DistributedAwareServiceConfiguration(receive);
			var profile       = new BlazorApplicationProfile(configuration.Execute, configure);
			return @this.Apply(profile);
		}
	}

	extension(Assembly[] @this)
	{
		public Action<IApplicationBuilder> Apply<T>() => @this.Apply<T>(_ => {});

		public Action<IApplicationBuilder> Apply<T>(Action<IApplicationBuilder> start)
			=> @this.Apply<T>(start, _ => {});

		public Action<IApplicationBuilder> Apply<T>(Action<IApplicationBuilder> start,
		                                            Action<IApplicationBuilder> post)
			=> new ApplyBlazorApplication(start, new ApplyBlazorWebApplication<T>(@this, post)).Execute;

		public ICommand<RazorComponentsEndpointConventionBuilder> AsConfiguration()
			=> new AddAdditionalAssemblies(@this);
	}
}