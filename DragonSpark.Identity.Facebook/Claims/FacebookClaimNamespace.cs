namespace DragonSpark.Identity.Facebook.Claims
{
	public sealed class FacebookClaimNamespace : Text.Text
	{
		public static FacebookClaimNamespace Default { get; } = new FacebookClaimNamespace();

		FacebookClaimNamespace() : base("urn:facebook") {}
	}
}