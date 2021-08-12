using AspNet.Security.OAuth.Amazon;
using DragonSpark.Model.Commands;
using System;

namespace DragonSpark.Identity.Amazon
{
	sealed class ConfigureAuthentication : ICommand<AmazonAuthenticationOptions>
	{
		readonly Func<AmazonApplicationSettings>   _settings;
		readonly Action<AmazonAuthenticationOptions> _configure;

		public ConfigureAuthentication(Func<AmazonApplicationSettings> settings,
		                               Action<AmazonAuthenticationOptions> configure)
		{
			_settings  = settings;
			_configure = configure;
		}

		public void Execute(AmazonAuthenticationOptions parameter)
		{
			var settings = _settings();

			parameter.ClientId     = settings.Key;
			parameter.ClientSecret = settings.Secret;

			_configure(parameter);
		}
	}
}