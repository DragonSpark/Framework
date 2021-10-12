using DragonSpark.Application.Security.Identity.Claims.Actions;

namespace DragonSpark.Identity.Mixcloud.Claims
{
	public class CloudCastsAction : ClaimAction
	{
		public static CloudCastsAction Default { get; } = new();

		CloudCastsAction() : base(CloudCasts.Default, "cloudcast_count", "integer") {}
	}
}