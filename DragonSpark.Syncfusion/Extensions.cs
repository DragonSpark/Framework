using DragonSpark.Application.AspNet.Compose;
using DragonSpark.Application.AspNet.Entities.Queries.Runtime.Shape;
using DragonSpark.Compose;
using Syncfusion.Blazor;

namespace DragonSpark.SyncfusionRendering;

public static class Extensions
{
	public static ApplicationProfileContext WithSyncfusion(this ApplicationProfileContext @this)
		=> @this.To(Configure.Default);

	public static Partition? Partition(this DataManagerRequest @this)
		=> @this.Skip > 0 || @this.Take > 0
			   ? new(@this.Skip > 0 ? @this.Skip : null, @this.Take > 0 ? @this.Take : null)
			   : null;
}