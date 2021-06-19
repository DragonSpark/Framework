using AspNet.Security.OAuth.Mixcloud;

namespace DragonSpark.Identity.Mixcloud
{
	public sealed class ProviderName : Text.Text
	{
		public static ProviderName Default { get; } = new ProviderName();

		ProviderName() : base(MixcloudAuthenticationDefaults.AuthenticationScheme) {}
	}
}