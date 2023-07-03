using AspNet.Security.OAuth.Twitter;
using DragonSpark.Application.Security.Identity.Claims.Actions;
using DragonSpark.Model.Commands;
using System;

namespace DragonSpark.Identity.Twitter;

sealed class ConfigureTwitterAuthentication : ICommand<TwitterAuthenticationOptions>
{
	readonly Func<TwitterApplicationSettings>     _settings;
	readonly IClaimAction                         _action;
	readonly Action<TwitterAuthenticationOptions> _configure;

	public ConfigureTwitterAuthentication(Func<TwitterApplicationSettings> settings, IClaimAction action,
	                                      Action<TwitterAuthenticationOptions> configure)
	{
		_settings  = settings;
		_action    = action;
		_configure = configure;
	}

	public void Execute(TwitterAuthenticationOptions parameter)
	{
		var settings = _settings();
		parameter.ClientId     = settings.Key;
		parameter.ClientSecret = settings.Secret;

		_action.Execute(parameter.ClaimActions);
		_configure(parameter);
	}
}