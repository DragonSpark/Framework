using AspNet.Security.OAuth.Reddit;
using DragonSpark.Application.AspNet.Security.Identity.Claims.Actions;
using DragonSpark.Composition;
using DragonSpark.Identity.Reddit.Claims;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Identity.Reddit;

sealed class ConfigureApplication : ICommand<AuthenticationBuilder>
{
	public static ConfigureApplication Default { get; } = new();

	ConfigureApplication() : this(DefaultClaimActions.Default, _ => {}) {}

	readonly IClaimAction                        _claims;
	readonly Action<RedditAuthenticationOptions> _configure;

	public ConfigureApplication(Action<RedditAuthenticationOptions> configure)
		: this(DefaultClaimActions.Default, configure) {}

	public ConfigureApplication(IClaimAction claims, Action<RedditAuthenticationOptions> configure)
	{
		_claims    = claims;
		_configure = configure;
	}

	public void Execute(AuthenticationBuilder parameter)
	{
		parameter.AddReddit()
		         .Services.Register<RedditApplicationSettings>()
		         .AddOptions<RedditAuthenticationOptions>(RedditAuthenticationDefaults.AuthenticationScheme)
		         .Configure<RedditApplicationSettings>((options, settings) =>
		                                               {
			                                               options.ClientId     = settings.Key;
			                                               options.ClientSecret = settings.Secret;

			                                               _claims.Execute(options.ClaimActions);
			                                               _configure(options);
		                                               });
	}
}