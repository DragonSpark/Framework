namespace DragonSpark.Identity.Twitter.Claims
{
	public class TwitterClaim : Text.Text
	{
		protected TwitterClaim(string name) : base($"{TwitterClaimNamespace.Default}:{name}") {}
	}
}