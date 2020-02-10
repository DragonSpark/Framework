namespace DragonSpark.Identity.Twitter
{
	public class TwitterClaim : Text.Text
	{
		protected TwitterClaim(string name) : base($"{TwitterClaimNamespace.Default}:{name}") {}
	}

	public sealed class TwitterClaimNamespace : Text.Text
	{
		public static TwitterClaimNamespace Default { get; } = new TwitterClaimNamespace();

		TwitterClaimNamespace() : base("urn:twitter") {}
	}
}