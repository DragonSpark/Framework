using AspNet.Security.OAuth.Paypal;
using DragonSpark.Application.Security.Identity.Claims.Actions;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Identity.PayPal;

sealed class ConfigureAuthentication : ICommand<PaypalAuthenticationOptions>
{
	readonly Func<PayPalApplicationSettings>     _settings;
	readonly IClaimAction                        _claim;
	readonly Action<PaypalAuthenticationOptions> _configure;

	public ConfigureAuthentication(IServiceCollection services, Action<PaypalAuthenticationOptions> configure)
		: this(services.Deferred<PayPalApplicationSettings>(), configure) {}

	public ConfigureAuthentication(Func<PayPalApplicationSettings> settings,
	                               Action<PaypalAuthenticationOptions> configure)
		: this(settings, ClaimActions.Default, configure) {}

	public ConfigureAuthentication(Func<PayPalApplicationSettings> settings, IClaimAction claim,
	                               Action<PaypalAuthenticationOptions> configure)
	{
		_settings  = settings;
		_claim     = claim;
		_configure = configure;
	}

	public void Execute(PaypalAuthenticationOptions parameter)
	{
		var settings = _settings();
		parameter.ClientId     = settings.Key;
		parameter.ClientSecret = settings.Secret;

		var authentication = settings.Authentication;
		parameter.AuthorizationEndpoint   = authentication.AuthorizationEndpoint;
		parameter.TokenEndpoint           = authentication.TokenEndpoint;
		parameter.UserInformationEndpoint = authentication.UserInformationEndpoint;

		_claim.Execute(parameter.ClaimActions);

		if (authentication.Scopes != null)
		{
			parameter.Scope.Clear();
			foreach (var scope in authentication.Scopes)
			{
				parameter.Scope.Add(scope);
			}
		}

		_configure(parameter);
	}
}