using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using System;

namespace DragonSpark.Identity.Microsoft
{
	sealed class ConfigureAuthentication : ICommand<MicrosoftAccountOptions>
	{
		readonly Func<MicrosoftApplicationSettings> _settings;

		public ConfigureAuthentication(Func<MicrosoftApplicationSettings> settings) => _settings = settings;

		public void Execute(MicrosoftAccountOptions parameter)
		{
			var settings = _settings();
			parameter.ClientId     = settings.Key;
			parameter.ClientSecret = settings.Secret;
		}
	}
}