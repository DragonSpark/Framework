using DragonSpark.Application.Compose;
using DragonSpark.Composition;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Security.Identity.Bearer;

public static class Extensions
{
	public static ApplicationProfileContext WithBearerSupport(this ApplicationProfileContext @this)
		=> @this.Append(Registrations.Default);

	public static IServiceCollection AddBearer(this IServiceCollection @this)
		=> @this.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
		        .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,
		                      new BearerConfiguration(@this.Deferred<BearerSettings>()).Execute)
		        .Services;
}