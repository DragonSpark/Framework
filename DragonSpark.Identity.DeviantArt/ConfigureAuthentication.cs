using AspNet.Security.OAuth.DeviantArt;
using DragonSpark.Application.Security.Identity.Claims.Actions;
using DragonSpark.Model.Commands;
using System;

namespace DragonSpark.Identity.DeviantArt;

sealed class ConfigureAuthentication : ICommand<DeviantArtAuthenticationOptions>
{
	readonly Func<DeviantArtApplicationSettings>     _settings;
	readonly IClaimAction                            _claims;
	readonly Action<DeviantArtAuthenticationOptions> _configure;

	public ConfigureAuthentication(Func<DeviantArtApplicationSettings> settings, IClaimAction claims,
	                               Action<DeviantArtAuthenticationOptions> configure)
	{
		_settings  = settings;
		_claims    = claims;
		_configure = configure;
	}

	public void Execute(DeviantArtAuthenticationOptions parameter)
	{
		var settings = _settings();

		parameter.ClientId                = settings.Key;
		parameter.ClientSecret            = settings.Secret;
		parameter.UserInformationEndpoint = settings.UserInformationEndpoint;

		_claims.Execute(parameter.ClaimActions);
		_configure(parameter);
	}
}