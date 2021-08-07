using AspNet.Security.OAuth.Patreon;

namespace DragonSpark.Identity.Patreon
{
	public sealed class ProviderName : Text.Text
	{
		public static ProviderName Default { get; } = new ProviderName();

		ProviderName() : base(PatreonAuthenticationDefaults.AuthenticationScheme) {}
	}
}