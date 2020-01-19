using DragonSpark.Composition.Compose;
using DragonSpark.Server;
using DragonSpark.Server.Compose;

namespace DragonSpark.Application.Hosting.Server.Blazor
{
	public static class Extensions
	{
		public static ServerProfileContext WithBlazorServerApplication(this BuildHostContext @this)
			=> @this.Apply(BlazorServerProfile.Default);
	}
}