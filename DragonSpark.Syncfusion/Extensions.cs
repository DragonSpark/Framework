using DragonSpark.Application.AspNet.Compose;
using DragonSpark.Compose;

namespace DragonSpark.SyncfusionRendering;

public static class Extensions
{
	public static ApplicationProfileContext WithSyncfusion(this ApplicationProfileContext @this)
		=> @this.To(Configure.Default);
}