using Microsoft.AspNetCore.Authorization;

namespace DragonSpark.Server.Mobile.Security.Devices.Validation;

public sealed class AuthorizeByValidatedDeviceAttribute : AuthorizeAttribute
{
    public AuthorizeByValidatedDeviceAttribute() : base(SchemeName.Default) {}
}