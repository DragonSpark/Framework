using DragonSpark.Application.AspNet.Security.Identity.Claims.Actions;
using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Identity.Google.Claims;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Identity.Google;

sealed class ConfigureApplication : ICommand<AuthenticationBuilder>
{
	readonly IClaimAction          _claims;
	readonly Action<GoogleOptions> _configure;

	public ConfigureApplication(Action<GoogleOptions> configure) : this(DefaultClaimActions.Default, configure) {}

	public ConfigureApplication(IClaimAction claims, Action<GoogleOptions> configure)
	{
		_claims    = claims;
		_configure = configure;
	}

	public void Execute(AuthenticationBuilder parameter)
	{
		var settings = parameter.Services.Deferred<GoogleApplicationSettings>();
		parameter.Services.Register<GoogleApplicationSettings>()
		         .Return(parameter)
		         .AddGoogle(new ConfigureAuthentication(settings, _claims, _configure).Execute)
			;
	}
}