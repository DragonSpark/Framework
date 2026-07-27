using Microsoft.AspNetCore.Authentication;

namespace DragonSpark.Server.Mobile.Security.Devices;

public sealed class DevicePoPOptions : AuthenticationSchemeOptions
{
    public TimeSpan MaxSkew { get; set; } = TimeSpan.FromSeconds(60);
    public bool RequireNonce { get; set; } = true;
}