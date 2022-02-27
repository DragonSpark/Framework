using AspNet.Security.OAuth.Yahoo;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Identity.Yahoo;

sealed class ConfigureApplication : ICommand<AuthenticationBuilder>
{
	readonly Action<YahooAuthenticationOptions> _configure;

	public ConfigureApplication(Action<YahooAuthenticationOptions> configure) => _configure = configure;

	public void Execute(AuthenticationBuilder parameter)
	{
		var settings = parameter.Services.Deferred<YahooApplicationSettings>();
		parameter.AddYahoo(new ConfigureAuthentication(settings, _configure).Execute);
		parameter.Services.Register<YahooApplicationSettings>();
	}
}