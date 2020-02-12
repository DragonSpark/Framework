using DragonSpark.Application.Compose;
using DragonSpark.Composition.Compose;
using DragonSpark.Server;

namespace DragonSpark.Application.Hosting.Server.Blazor
{
	public static class Extensions
	{
		public static ApplicationProfileContext WithBlazorServerApplication(this BuildHostContext @this)
			=> @this.Apply(BlazorApplicationProfile.Default);
	}
}