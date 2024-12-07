using DragonSpark.Composition.Compose;

namespace DragonSpark.Application.Communication;

public static class Extensions
{
	public static BuildHostContext WithClientState(this BuildHostContext @this)
		=> @this.Configure(Registrations.Default);
}