using AspNet.Security.OAuth.Discord;
using DragonSpark.Model.Commands;
using System;

namespace DragonSpark.Identity.Discord;

sealed class ConfigureAuthentication : ICommand<DiscordAuthenticationOptions>
{
	readonly Func<DiscordIdentitySettings>        _settings;
	readonly Action<DiscordAuthenticationOptions> _configure;

	public ConfigureAuthentication(Func<DiscordIdentitySettings> settings,
	                               Action<DiscordAuthenticationOptions> configure)
	{
		_settings  = settings;
		_configure = configure;
	}

	public void Execute(DiscordAuthenticationOptions parameter)
	{
		var settings = _settings();

		parameter.ClientId     = settings.Key;
		parameter.ClientSecret = settings.Secret;

		_configure(parameter);
	}
}