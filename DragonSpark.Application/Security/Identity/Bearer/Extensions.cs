using DragonSpark.Application.Compose;
using DragonSpark.Composition;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Security.Identity.Bearer;

public static class Extensions
{
	public static ApplicationProfileContext WithBearerSupport(this ApplicationProfileContext @this)
		=> @this.Append(Registrations.Default);

	public static AuthenticationBuilder AddBearer(this AuthenticationBuilder @this)
		=> @this.AddJwtBearer(new BearerConfiguration(@this.Services.Deferred<BearerSettings>()).Execute);
}