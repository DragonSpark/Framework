using DragonSpark.Application.AspNet;
using DragonSpark.Application.AspNet.Compose;
using DragonSpark.Composition.Compose;
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
			=> @this.Apply(new BlazorApplicationProfile<T>(builder, additional));

		public ApplicationProfileContext WithOptimizedBlazorServerApplication<T>(Action<IApplicationBuilder> builder,
		                                                                         byte receive = 32,
		                                                                         params Assembly[] additional)
		{
			var configuration = new DistributedAwareServiceConfiguration(receive);
			var profile       = new BlazorApplicationProfile<T>(configuration.Execute, builder, additional);
			return @this.Apply(profile);
		}
	}
}