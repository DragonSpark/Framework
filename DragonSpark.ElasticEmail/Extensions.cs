using DragonSpark.Application.Compose;
using DragonSpark.Compose;

namespace DragonSpark.ElasticEmail
{
	public static class Extensions
	{
		public static ApplicationProfileContext WithElasticEmail(this ApplicationProfileContext @this)
			=> @this.To(Configure.Default);
	}
}