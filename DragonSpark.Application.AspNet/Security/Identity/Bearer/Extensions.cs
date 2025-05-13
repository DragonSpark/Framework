using DragonSpark.Application.AspNet.Compose;
using DragonSpark.Compose;
using DragonSpark.Composition;
using Humanizer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.AspNet.Security.Identity.Bearer;

public static class Extensions
{
	public static ApplicationProfileContext WithBearerSupport(this ApplicationProfileContext @this)
		=> @this.Append(DragonSpark.Application.Security.Identity.Bearer.Registrations.Default, Registrations.Default);

	public static AuthenticationBuilder AddDefaultBearer(this IServiceCollection @this,
	                                                     string name = JwtBearerDefaults.AuthenticationScheme)
		=> @this.AddAuthentication(x => x.DefaultScheme = x.DefaultChallengeScheme = name)
		        .AddJwtBearer(x => @this.Configuration().Bind(nameof(JwtBearerOptions), x));

	public static AuthenticationBuilder AddBearer(this IServiceCollection @this,
	                                              string name = nameof(BearerAwarePolicyScheme))
		=> @this.AddAuthentication(x => x.DefaultScheme = x.DefaultChallengeScheme = name)
		        .AddJwtBearer(@this.Deferred<BearerConfiguration>().Assume())
		        .AddPolicyScheme(name, name.Humanize(), BearerAwarePolicyScheme.Default.Execute);
}