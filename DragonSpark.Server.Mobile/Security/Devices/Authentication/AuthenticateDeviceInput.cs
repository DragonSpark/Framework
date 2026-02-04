using Microsoft.AspNetCore.Http;

namespace DragonSpark.Server.Mobile.Security.Devices.Authentication;

public readonly record struct AuthenticateDeviceInput(HttpContext Context, string Scheme);