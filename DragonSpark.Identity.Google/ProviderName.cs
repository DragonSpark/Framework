using Microsoft.AspNetCore.Authentication.Google;

namespace DragonSpark.Identity.Google
{
	public sealed class ProviderName : Text.Text
	{
		public static ProviderName Default { get; } = new ProviderName();

		ProviderName() : base(GoogleDefaults.AuthenticationScheme) {}
	}
}