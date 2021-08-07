namespace DragonSpark.Identity.Patreon.Claims
{
	public sealed class PatreonClaimNamespace : Text.Text
	{
		public static PatreonClaimNamespace Default { get; } = new PatreonClaimNamespace();

		PatreonClaimNamespace() : base("urn:patreon") {}
	}
}