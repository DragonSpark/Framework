using DragonSpark.Application.Compose;
using DragonSpark.Composition.Compose;

namespace DragonSpark.Application.Hosting.Server
{
	public static class Extension
	{
		public static ApplicationProfileContext WithServerApplication(this BuildHostContext @this)
			=> @this.Apply(ServerApplicationProfile.Default);
	}
}