using AspNet.Security.OAuth.Mixcloud;
using DragonSpark.Application.Security.Identity.Claims.Actions;
using DragonSpark.Model.Commands;
using System;

namespace DragonSpark.Identity.Mixcloud
{
	sealed class ConfigureAuthentication : ICommand<MixcloudAuthenticationOptions>
	{
		readonly Func<MixcloudApplicationSettings> _settings;
		readonly IClaimAction                      _claims;

		public ConfigureAuthentication(Func<MixcloudApplicationSettings> settings, IClaimAction claims)
		{
			_settings = settings;
			_claims   = claims;
		}

		public void Execute(MixcloudAuthenticationOptions parameter)
		{
			var settings = _settings();
			parameter.ClientId     = settings.Key;
			parameter.ClientSecret = settings.Secret;

			_claims.Execute(parameter.ClaimActions);
		}
	}
}