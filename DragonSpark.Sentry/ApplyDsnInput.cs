using Microsoft.Extensions.Configuration;

namespace DragonSpark.Sentry;

public readonly record struct ApplyDsnInput(IConfiguration Configuration, string? Name);