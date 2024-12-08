using DragonSpark.Composition.Compose;
using JetBrains.Annotations;

namespace DragonSpark.Application.AspNet.Communication;

public static class Extensions
{
	[UsedImplicitly]
	public static BuildHostContext WithClientState(this BuildHostContext @this)
		=> @this.Configure(Registrations.Default);
}