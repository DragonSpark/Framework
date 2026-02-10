using DragonSpark.Application.Security.Tokens;

namespace DragonSpark.Server.Mobile.Security.Devices;

public sealed class AuthorizeByProvenDeviceAttribute : Microsoft.AspNetCore.Authorization.AuthorizeAttribute
{
    public AuthorizeByProvenDeviceAttribute() : base(SchemeName.Default) {}
}