using AspNet.Security.OAuth.DeviantArt;

namespace DragonSpark.Identity.DeviantArt
{
	public sealed class ProviderName : Text.Text
	{
		public static ProviderName Default { get; } = new ProviderName();

		ProviderName() : base(DeviantArtAuthenticationDefaults.AuthenticationScheme) {}
	}
}