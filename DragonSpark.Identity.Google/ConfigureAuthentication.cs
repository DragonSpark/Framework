using DragonSpark.Application.Security.Identity.Claims.Actions;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Authentication.Google;
using System;

namespace DragonSpark.Identity.Google
{
	sealed class ConfigureAuthentication : ICommand<GoogleOptions>
	{
		readonly Func<GoogleApplicationSettings> _settings;
		readonly IClaimAction                    _claims;

		public ConfigureAuthentication(Func<GoogleApplicationSettings> settings, IClaimAction claims)
		{
			_settings = settings;
			_claims   = claims;
		}

		public void Execute(GoogleOptions parameter)
		{
			var settings = _settings();
			parameter.ClientId     = settings.Key;
			parameter.ClientSecret = settings.Secret;

			_claims.Execute(parameter.ClaimActions);
		}
	}
}