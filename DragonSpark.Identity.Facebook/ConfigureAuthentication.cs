using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Authentication.Facebook;
using System;

namespace DragonSpark.Identity.Facebook
{
	sealed class ConfigureAuthentication : ICommand<FacebookOptions>
	{
		readonly Func<FacebookApplicationSettings> _settings;
		readonly Action<FacebookOptions>           _configure;

		public ConfigureAuthentication(Func<FacebookApplicationSettings> settings, Action<FacebookOptions> configure)
		{
			_settings  = settings;
			_configure = configure;
		}

		public void Execute(FacebookOptions parameter)
		{
			var settings = _settings();
			parameter.ClientId     = settings.Key;
			parameter.ClientSecret = settings.Secret;

			_configure(parameter);
		}
	}
}