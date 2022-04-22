using DragonSpark.Compose;
using Microsoft.AspNetCore.Builder;

namespace DragonSpark.Server.Requests.Warmup;

public static class Extensions
{
	public static IApplicationBuilder UseWarmupAwareHttpsRedirection(this IApplicationBuilder @this)
		=> WarmupAwareHttpsRedirection.Default.Parameter(@this);
}