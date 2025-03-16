using Microsoft.AspNetCore.Builder;

namespace DragonSpark.Application.AspNet.Middleware;

public static class ApplicationBuilderExtensions
{
	public static IApplicationBuilder RequireHttps(this IApplicationBuilder @this)
		=> @this.Use(RequireHttpsMiddleware.Default.Get);

	public static IApplicationBuilder OutputCacheSuccessfulRequestsOnly(this IApplicationBuilder @this)
		=> @this.Use(OutputCacheSuccessfulRequestsOnlyMiddleware.Default.Get);
}