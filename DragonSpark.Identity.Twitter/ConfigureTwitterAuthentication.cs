using DragonSpark.Application.Security.Identity.Claims.Actions;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Authentication.Twitter;
using System;

namespace DragonSpark.Identity.Twitter;

sealed class ConfigureTwitterAuthentication : ICommand<TwitterOptions>
{
	readonly Func<TwitterApplicationSettings> _settings;
	readonly IClaimAction                     _action;
	readonly Action<TwitterOptions>           _configure;

	public ConfigureTwitterAuthentication(Func<TwitterApplicationSettings> settings, IClaimAction action,
	                                      Action<TwitterOptions> configure)
	{
		_settings  = settings;
		_action    = action;
		_configure = configure;
	}

	public void Execute(TwitterOptions parameter)
	{
		var settings = _settings();

		parameter.ConsumerKey         = settings.Key;
		parameter.ConsumerSecret      = settings.Secret;
		parameter.RetrieveUserDetails = true;

		_action.Execute(parameter.ClaimActions);
		_configure(parameter);
	}
}