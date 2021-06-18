namespace DragonSpark.Identity.Reddit.Claims
{
	public class RedditClaim : Text.Text
	{
		protected RedditClaim(string name) : base($"{RedditClaimNamespace.Default}:{name}") {}
	}
}