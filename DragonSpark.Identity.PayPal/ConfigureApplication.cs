using AspNet.Security.OAuth.Paypal;
using DragonSpark.Compose;
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
		var settings = parameter.Services.Deferred<PayPalApplicationSettings>();
		parameter.Services.Register<PayPalApplicationSettings>()
		         .Return(parameter)
		         .AddPaypal(new ConfigureAuthentication(settings, _configure).Execute);
	}
}