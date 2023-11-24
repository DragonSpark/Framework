using Microsoft.AspNetCore.Authentication.Facebook;

namespace DragonSpark.Identity.Facebook;

public sealed class ProviderName : Text.Text
{
	public static ProviderName Default { get; } = new();

	ProviderName() : base(FacebookDefaults.AuthenticationScheme) {}
}