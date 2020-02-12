using DragonSpark.Application.Compose;
using DragonSpark.Composition.Compose;

namespace DragonSpark.Server
{
	public static class Extensions
	{
		public static ApplicationProfileContext Apply(this BuildHostContext @this, IServerProfile profile)
			=> new ApplicationProfileContext(@this, profile);
	}
}