using AspNet.Security.OAuth.Yahoo;
using DragonSpark.Model.Commands;
using System;

namespace DragonSpark.Identity.Yahoo;

sealed class ConfigureAuthentication : ICommand<YahooAuthenticationOptions>
{
	readonly Func<YahooApplicationSettings>     _settings;
	readonly Action<YahooAuthenticationOptions> _configure;

	public ConfigureAuthentication(Func<YahooApplicationSettings> settings,
	                               Action<YahooAuthenticationOptions> configure)
	{
		_settings  = settings;
		_configure = configure;
	}

	public void Execute(YahooAuthenticationOptions parameter)
	{
		var settings = _settings();

		parameter.ClientId     = settings.Key;
		parameter.ClientSecret = settings.Secret;

		_configure(parameter);
	}
}