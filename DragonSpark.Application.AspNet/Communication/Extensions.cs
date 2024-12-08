using DragonSpark.Composition.Compose;

namespace DragonSpark.Application.AspNet.Communication;

public static class Extensions
{
	public static BuildHostContext WithClientState(this BuildHostContext @this)
		=> @this.Configure(RegistrationsUndo.Default);
}