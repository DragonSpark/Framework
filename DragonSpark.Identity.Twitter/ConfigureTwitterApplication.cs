using AspNet.Security.OAuth.Twitter;
using DragonSpark.Application.AspNet.Security.Identity.Claims.Actions;
using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace DragonSpark.Identity.Twitter;

sealed class ConfigureTwitterApplication : ICommand<AuthenticationBuilder>
{
	readonly IClaimAction                         _action;
	readonly Action<TwitterAuthenticationOptions> _configure;

	public ConfigureTwitterApplication(IClaimAction action, Action<TwitterAuthenticationOptions> configure)
	{
		_action    = action;
		_configure = configure;
	}

	public void Execute(AuthenticationBuilder parameter)
	{
		var settings       = parameter.Services.Deferred<TwitterApplicationSettings>();
		var authentication = new ConfigureTwitterAuthentication(settings, _action, _configure);
		parameter.Services.Register<TwitterApplicationSettings>()
		         .Return(parameter)
		         .AddTwitter(authentication.Execute)
		         //
		         .Services
		         .AddSingleton<IPostConfigureOptions<TwitterAuthenticationOptions>,
			         PostConfigureAuthenticationOptions>()
			;
	}
}