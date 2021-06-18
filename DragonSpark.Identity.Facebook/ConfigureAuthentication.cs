using DragonSpark.Application.Security;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Authentication.Facebook;
using System;

namespace DragonSpark.Identity.Facebook
{
	sealed class ConfigureAuthentication : ICommand<FacebookOptions>
	{
		readonly Func<FacebookApplicationSettings> _settings;
		readonly IClaimAction                      _claims;

		public ConfigureAuthentication(Func<FacebookApplicationSettings> settings, IClaimAction claims)
		{
			_settings = settings;
			_claims   = claims;
		}

		public void Execute(FacebookOptions parameter)
		{
			var settings = _settings();
			parameter.ClientId     = settings.Key;
			parameter.ClientSecret = settings.Secret;

			_claims.Execute(parameter.ClaimActions);
		}
	}
}