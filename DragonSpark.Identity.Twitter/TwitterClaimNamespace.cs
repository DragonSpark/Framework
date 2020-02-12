namespace DragonSpark.Identity.Twitter
{
	public sealed class TwitterClaimNamespace : Text.Text
	{
		public static TwitterClaimNamespace Default { get; } = new TwitterClaimNamespace();

		TwitterClaimNamespace() : base("urn:twitter") {}
	}

	public sealed class Claims : Application.Security.Identity.Claims
	{
		public static Claims Default { get; } = new Claims();

		Claims() : base(x => x.Type.StartsWith(TwitterClaimNamespace.Default)) {}
	}
}