using DragonSpark.Compose;
using Microsoft.Extensions.Hosting;

namespace DragonSpark.Sentry;

public static class Extensions
{
	public static IHostApplicationBuilder WithSentry(this IHostApplicationBuilder @this, string? name = null)
		=> new ConfigureSentry(name).Parameter(@this);
}