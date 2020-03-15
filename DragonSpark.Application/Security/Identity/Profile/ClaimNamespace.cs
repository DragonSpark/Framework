namespace DragonSpark.Application.Security.Identity.Profile {
	public sealed class ClaimNamespace : Text.Text
	{
		public static ClaimNamespace Default { get; } = new ClaimNamespace();

		ClaimNamespace() : base("urn:dragonspark") {}
	}
}