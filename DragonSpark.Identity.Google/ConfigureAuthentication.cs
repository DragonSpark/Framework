using DragonSpark.Application.AspNet.Security.Identity.Claims.Actions;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Authentication.Google;
using System;

namespace DragonSpark.Identity.Google;

sealed class ConfigureAuthentication : ICommand<GoogleOptions>
{
	readonly Func<GoogleApplicationSettings> _settings;
	readonly IClaimAction                    _claims;
	readonly Action<GoogleOptions>           _configure;

	public ConfigureAuthentication(Func<GoogleApplicationSettings> settings, IClaimAction claims, Action<GoogleOptions> configure)
	{
		_settings       = settings;
		_claims         = claims;
		_configure = configure;
	}

	public void Execute(GoogleOptions parameter)
	{
		var settings = _settings();
		parameter.ClientId     = settings.Key;
		parameter.ClientSecret = settings.Secret;

		_claims.Execute(parameter.ClaimActions);
		_configure(parameter);
	}
}