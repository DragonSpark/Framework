using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Identity.Microsoft;

sealed class ConfigureApplication : ICommand<AuthenticationBuilder>
{
	readonly Action<MicrosoftAccountOptions> _configure;

	public ConfigureApplication(Action<MicrosoftAccountOptions> configure) => _configure = configure;

	public void Execute(AuthenticationBuilder parameter)
	{
		var settings = parameter.Services.Deferred<MicrosoftApplicationSettings>();
		parameter.Services.Register<MicrosoftApplicationSettings>()
		         .Return(parameter)
		         .AddMicrosoftAccount(new ConfigureAuthentication(settings, _configure).Execute)
			;
	}
}