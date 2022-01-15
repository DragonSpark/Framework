using Microsoft.AspNetCore.Authentication.MicrosoftAccount;

namespace DragonSpark.Identity.Microsoft;

public sealed class MicrosoftApplicationSettings
{
	public string Key { get; set; }  = null!;

	public string Secret { get; set; }  = null!;

	public string AuthorizationEndpoint { get; set; } = MicrosoftAccountDefaults.AuthorizationEndpoint;

	public string TokenEndpoint { get; set; } = MicrosoftAccountDefaults.TokenEndpoint;
}