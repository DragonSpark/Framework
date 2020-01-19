using DragonSpark.Composition.Compose;

namespace DragonSpark.Testing.Server
{
	public static class ExtensionMethods
	{
		public static BuildHostContext WithTestServer(this BuildHostContext @this)
			=> @this.Configure(ServerConfiguration.Default);
	}
}