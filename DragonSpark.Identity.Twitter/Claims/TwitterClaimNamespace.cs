namespace DragonSpark.Identity.Twitter.Claims
{
	public sealed class TwitterClaimNamespace : Text.Text
	{
		public static TwitterClaimNamespace Default { get; } = new TwitterClaimNamespace();

		TwitterClaimNamespace() : base("urn:twitter") {}
	}
}