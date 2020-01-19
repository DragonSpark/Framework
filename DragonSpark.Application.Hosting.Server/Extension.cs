using DragonSpark.Composition.Compose;
using DragonSpark.Server;
using DragonSpark.Server.Compose;

namespace DragonSpark.Application.Hosting.Server {
	public static class Extension
	{
		public static ServerProfileContext WithServerApplication(this BuildHostContext @this)
			=> @this.Apply(ServerApplicationProfile.Default);
	}
}