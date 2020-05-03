using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Authentication.Google;
using System;

namespace DragonSpark.Identity.Google
{
	sealed class ConfigureAuthentication : ICommand<GoogleOptions>
	{
		readonly Func<GoogleApplicationSettings> _settings;

		public ConfigureAuthentication(Func<GoogleApplicationSettings> settings) => _settings = settings;

		public void Execute(GoogleOptions parameter)
		{
			var settings = _settings();
			parameter.ClientId     = settings.Key;
			parameter.ClientSecret = settings.Secret;
		}
	}
}