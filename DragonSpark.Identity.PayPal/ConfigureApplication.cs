using AspNet.Security.OAuth.Paypal;
using DragonSpark.Application.AspNet.Security.Identity.Claims.Compile;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Identity.PayPal;

sealed class ConfigureApplication : ICommand<AuthenticationBuilder>
{
	readonly Action<PaypalAuthenticationOptions> _configure;

	public ConfigureApplication(Action<PaypalAuthenticationOptions> configure) => _configure = configure;

	public void Execute(AuthenticationBuilder parameter)
	{
		var services = parameter.Services;
		services.Register<PayPalApplicationSettings>();
		services.TryDecorate<IClaims, Claims>();
		services.TryDecorate<IKnownClaims, AdditionalClaims>();
		parameter.AddPaypal(new ConfigureAuthentication(services, _configure).Execute);
	}
}