namespace DragonSpark.Identity.Reddit.Claims
{
	public sealed class RedditClaimNamespace : Text.Text
	{
		public static RedditClaimNamespace Default { get; } = new RedditClaimNamespace();

		RedditClaimNamespace() : base("urn:reddit") {}
	}
}