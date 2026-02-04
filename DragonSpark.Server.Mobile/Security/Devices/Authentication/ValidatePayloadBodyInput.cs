using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Server.Mobile.Security.Devices.Authentication;

public readonly record struct ValidatePayloadBodyInput(
    HttpRequest Request,
    JsonElement Root,
    string? Address,
    long iat);