using DragonSpark.Composition.Compose;
using DragonSpark.Server.Application;
using DragonSpark.Server.Compose;

namespace DragonSpark.Server
{
	public static class Extensions
	{
		public static ServerProfileContext Apply(this BuildHostContext @this, IServerProfile profile)
			=> new ServerProfileContext(@this, profile);

	}
}