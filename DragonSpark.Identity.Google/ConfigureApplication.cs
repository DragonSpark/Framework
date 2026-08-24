using DragonSpark.Application.AspNet.Security.Identity.Claims.Actions;
using DragonSpark.Composition;
using DragonSpark.Identity.Google.Claims;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Identity.Google;

sealed class ConfigureApplication : ICommand<AuthenticationBuilder>
{
	readonly IClaimAction          _claims;
	readonly Action<GoogleOptions> _configure;

	public ConfigureApplication(Action<GoogleOptions> configure) : this(DefaultClaimActions.Default, configure) {}

	public ConfigureApplication(IClaimAction claims, Action<GoogleOptions> configure)
	{
		_claims    = claims;
		_configure = configure;
	}

	public void Execute(AuthenticationBuilder parameter)
	{
		parameter.AddGoogle()
		         .Services.Register<GoogleApplicationSettings>()
		         .AddOptions<GoogleOptions>(GoogleDefaults.AuthenticationScheme)
		         .Configure<GoogleApplicationSettings>((options, settings) =>
		                                               {
			                                               options.ClientId     = settings.Key;
			                                               options.ClientSecret = settings.Secret;

			                                               _claims.Execute(options.ClaimActions);
			                                               _configure(options);
		                                               });
	}
}