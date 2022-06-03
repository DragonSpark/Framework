using AspNet.Security.OAuth.Paypal;
using DragonSpark.Application;
using DragonSpark.Application.Security.Identity.Authentication;
using DragonSpark.Application.Security.Identity.Claims.Compile;
using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace DragonSpark.Identity.PayPal;

sealed class ConfigureApplication : ICommand<AuthenticationBuilder>
{
	readonly Action<PaypalAuthenticationOptions> _configure;

	public ConfigureApplication(Action<PaypalAuthenticationOptions> configure) => _configure = configure;

	public void Execute(AuthenticationBuilder parameter)
	{
		var settings = parameter.Services.Deferred<PayPalApplicationSettings>();
		parameter.Services.Register<PayPalApplicationSettings>()
		         .TryDecorate<IClaims, Claims>()
		         .Return(parameter)
		         .AddPaypal(new ConfigureAuthentication(settings, _configure).Execute);
	}
}

// TODO:

sealed class Claims : IClaims
{
	readonly IClaims _previous;
	readonly string  _name;

	public Claims(IClaims previous) : this(previous, PayIdentifier.Default) {}

	public Claims(IClaims previous, string name)
	{
		_previous = previous;
		_name     = name;
	}

	public IEnumerable<Claim> Get(Login parameter)
	{
		var (principal, provider, _) = parameter;
		var previous = _previous.Get(parameter);
		var result = provider == PaypalAuthenticationDefaults.AuthenticationScheme && principal.HasClaim(_name)
			             ? previous.Append(new Claim(_name, principal.FindFirstValue(_name)))
			             : previous;
		return result;
	}
}