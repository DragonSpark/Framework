using System;

namespace DragonSpark.Application.AspNet.Security.Identity.Bearer;

public sealed record MessageBearerSettings
{
    public TimeSpan Expires { get; set; } = TimeSpan.FromMinutes(1);
}