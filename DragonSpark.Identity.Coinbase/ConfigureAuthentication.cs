using AspNet.Security.OAuth.Coinbase;
using DragonSpark.Model.Commands;
using System;

namespace DragonSpark.Identity.Coinbase
{
	sealed class ConfigureAuthentication : ICommand<CoinbaseAuthenticationOptions>
	{
		readonly Func<CoinbaseApplicationSettings>     _settings;
		readonly Action<CoinbaseAuthenticationOptions> _configure;

		public ConfigureAuthentication(Func<CoinbaseApplicationSettings> settings,
		                               Action<CoinbaseAuthenticationOptions> configure)
		{
			_settings  = settings;
			_configure = configure;
		}

		public void Execute(CoinbaseAuthenticationOptions parameter)
		{
			var settings = _settings();

			parameter.ClientId     = settings.Key;
			parameter.ClientSecret = settings.Secret;

			_configure(parameter);
		}
	}
}