using System;
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

    public static IServiceCollection WithBearerSupport(this IServiceCollection @this)
        => @this.Configured(DragonSpark.Application.Security.Identity.Bearer.Registrations.Default,
                            Registrations.Default);

    public static AuthenticationBuilder AddDefaultBearer(this IServiceCollection @this,
                                                         string name = JwtBearerDefaults.AuthenticationScheme)
        => @this.AddDefaultBearer(_ => {}, name);

    public static AuthenticationBuilder AddDefaultBearer(this IServiceCollection @this,
                                                         Action<JwtBearerOptions> configure,
                                                         string name = JwtBearerDefaults.AuthenticationScheme)
        => @this.AddAuthentication(x => x.DefaultScheme = x.DefaultChallengeScheme = name)
                .AddJwtBearer(x =>
                              {
                                  @this.Configuration().Bind(nameof(JwtBearerOptions), x);
                                  configure(x);
                              });

    public static AuthenticationBuilder AddBearer(this IServiceCollection @this) => @this.AddBearer(_ => {});

    public static AuthenticationBuilder AddBearer(this IServiceCollection @this, Action<JwtBearerOptions> configure)
        => @this.AddBearer(configure, IdentityApplicationPolicySelector.Default);

    public static AuthenticationBuilder AddBearer(this IServiceCollection @this, Action<JwtBearerOptions> configure,
                                                  IPolicySelector selector)
    {
        var name = selector.GetType().Name;
        return @this.AddAuthentication(x => x.DefaultScheme = x.DefaultChallengeScheme = name)
                    .AddJwtBearer(@this.Deferred<BearerConfiguration>().Assume().Append(configure))
                    .AddPolicyScheme(name, name.Humanize(LetterCasing.Title), new PolicyScheme(selector).Execute);
    }
}