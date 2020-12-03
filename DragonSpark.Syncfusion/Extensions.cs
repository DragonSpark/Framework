using DragonSpark.Application.Compose;
using DragonSpark.Compose;

namespace DragonSpark.Syncfusion
{
	public static class Extensions
	{
		public static ApplicationProfileContext WithSyncfusion(this ApplicationProfileContext @this)
			=> @this.To(Configure.Default);
	}
}