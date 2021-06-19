namespace DragonSpark.Identity.Mixcloud.Claims
{
	public class MixcloudClaim : Text.Text
	{
		protected MixcloudClaim(string name) : base($"{MixcloudClaimNamespace.Default}:{name}") {}
	}
}