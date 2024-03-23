using DragonSpark.Application.Compose;
using DragonSpark.Compose;
using JetBrains.Annotations;

namespace DragonSpark.ElasticEmail;

public static class Extensions
{
	[UsedImplicitly]
	public static ApplicationProfileContext WithElasticEmail(this ApplicationProfileContext @this)
		=> @this.To(Configure.Default);
}