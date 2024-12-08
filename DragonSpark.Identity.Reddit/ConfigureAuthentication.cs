using AspNet.Security.OAuth.Reddit;
using DragonSpark.Application.AspNet.Security.Identity.Claims.Actions;
using DragonSpark.Model.Commands;
using System;

namespace DragonSpark.Identity.Reddit;

sealed class ConfigureAuthentication : ICommand<RedditAuthenticationOptions>
{
	readonly Func<RedditApplicationSettings>     _settings;
	readonly IClaimAction                        _claims;
	readonly Action<RedditAuthenticationOptions> _configure;

	public ConfigureAuthentication(Func<RedditApplicationSettings> settings, IClaimAction claims,
	                               Action<RedditAuthenticationOptions> configure)
	{
		_settings       = settings;
		_claims         = claims;
		_configure = configure;
	}

	public void Execute(RedditAuthenticationOptions parameter)
	{
		var settings = _settings();
		parameter.ClientId     = settings.Key;
		parameter.ClientSecret = settings.Secret;

		_claims.Execute(parameter.ClaimActions);
		_configure(parameter);
	}
}