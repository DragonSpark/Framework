using DragonSpark.Application.Compose;
using DragonSpark.Composition.Compose;

namespace DragonSpark.Server
{
	public static class Extensions
	{
		public static ServerProfileContext Apply(this BuildHostContext @this, IServerProfile profile)
			=> new ServerProfileContext(@this, profile);
	}
}