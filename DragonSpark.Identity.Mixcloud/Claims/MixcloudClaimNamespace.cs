namespace DragonSpark.Identity.Mixcloud.Claims
{
	public sealed class MixcloudClaimNamespace : Text.Text
	{
		public static MixcloudClaimNamespace Default { get; } = new MixcloudClaimNamespace();

		MixcloudClaimNamespace() : base("urn:mixcloud") {}
	}
}