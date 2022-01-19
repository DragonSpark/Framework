using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using System;

namespace DragonSpark.Identity.Microsoft;

sealed class ConfigureAuthentication : ICommand<MicrosoftAccountOptions>
{
	readonly Func<MicrosoftApplicationSettings> _settings;
	readonly Action<MicrosoftAccountOptions>    _configure;

	public ConfigureAuthentication(Func<MicrosoftApplicationSettings> settings,
	                               Action<MicrosoftAccountOptions> configure)
	{
		_settings  = settings;
		_configure = configure;
	}

	public void Execute(MicrosoftAccountOptions parameter)
	{
		var settings = _settings();
		parameter.ClientId              = settings.Key;
		parameter.ClientSecret          = settings.Secret;
		parameter.AuthorizationEndpoint = settings.AuthorizationEndpoint;
		parameter.TokenEndpoint         = settings.TokenEndpoint;
		_configure(parameter);
	}
}