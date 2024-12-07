using DragonSpark.Application.Compose;
using DragonSpark.Compose;
using DragonSpark.Composition;
using Humanizer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Security.Identity.Bearer;

public static class Extensions
{
	public static ApplicationProfileContext WithBearerSupport(this ApplicationProfileContext @this)
		=> @this.Append(Registrations.Default);

	public static AuthenticationBuilder AddBearer(this IServiceCollection @this)
	{
		const string name = nameof(BearerAwarePolicyScheme);
		return @this.AddAuthentication(x => x.DefaultScheme = x.DefaultChallengeScheme = name)
		            .AddJwtBearer(@this.Deferred<BearerConfiguration>().Assume())
		            .AddPolicyScheme(name, name.Humanize(), BearerAwarePolicyScheme.Default.Execute);
	}
}