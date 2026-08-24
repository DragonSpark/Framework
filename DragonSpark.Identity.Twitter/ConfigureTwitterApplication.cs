using AspNet.Security.OAuth.Twitter;
using DragonSpark.Application.AspNet.Security.Identity.Claims.Actions;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace DragonSpark.Identity.Twitter;

sealed class ConfigureTwitterApplication : ICommand<AuthenticationBuilder>
{
	readonly IClaimAction                         _action;
	readonly Action<TwitterAuthenticationOptions> _configure;

	public ConfigureTwitterApplication(IClaimAction action, Action<TwitterAuthenticationOptions> configure)
	{
		_action    = action;
		_configure = configure;
	}

	public void Execute(AuthenticationBuilder parameter)
	{
		parameter.AddTwitter()
		         .Services.Register<TwitterApplicationSettings>()
		         .AddOptions<TwitterAuthenticationOptions>(TwitterAuthenticationDefaults.AuthenticationScheme)
		         .Configure<TwitterApplicationSettings>((options, settings) =>
		                                                {
			                                                options.ClientId     = settings.Key;
			                                                options.ClientSecret = settings.Secret;

			                                                _action.Execute(options.ClaimActions);
			                                                _configure(options);
		                                                });

		parameter.Services.AddSingleton<IPostConfigureOptions<TwitterAuthenticationOptions>,
			PostConfigureAuthenticationOptions>();
	}
}