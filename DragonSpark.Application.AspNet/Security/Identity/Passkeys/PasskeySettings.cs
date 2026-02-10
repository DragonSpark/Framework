using Microsoft.AspNetCore.Http;

namespace DragonSpark.Application.AspNet.Security.Identity.Passkeys;

public sealed record PasskeySettings
{
    public required string Name { get; set; }

    public required PathString LoginPath { get; set; }
}