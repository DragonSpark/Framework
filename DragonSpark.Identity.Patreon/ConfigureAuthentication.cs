using AspNet.Security.OAuth.Patreon;
using DragonSpark.Model.Commands;
using System;

namespace DragonSpark.Identity.Patreon
{
	sealed class ConfigureAuthentication : ICommand<PatreonAuthenticationOptions>
	{
		readonly Func<PatreonApplicationSettings>    _settings;
		readonly Action<PatreonAuthenticationOptions> _configure;

		public ConfigureAuthentication(Func<PatreonApplicationSettings> settings,
		                               Action<PatreonAuthenticationOptions> configure)
		{
			_settings  = settings;
			_configure = configure;
		}

		public void Execute(PatreonAuthenticationOptions parameter)
		{
			var settings = _settings();
			parameter.ClientId     = settings.Key;
			parameter.ClientSecret = settings.Secret;

			_configure(parameter);
		}
	}
}