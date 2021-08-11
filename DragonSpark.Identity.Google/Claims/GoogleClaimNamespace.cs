namespace DragonSpark.Identity.Google.Claims
{
	public sealed class GoogleClaimNamespace : Text.Text
	{
		public static GoogleClaimNamespace Default { get; } = new GoogleClaimNamespace();

		GoogleClaimNamespace() : base("urn:deviantart") {}
	}
}