using AspNet.Security.OAuth.Discord;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Identity.Discord;

sealed class ConfigureApplication : ICommand<AuthenticationBuilder>
{
	readonly Action<DiscordAuthenticationOptions> _configure;

	public ConfigureApplication(Action<DiscordAuthenticationOptions> configure) => _configure = configure;

	public void Execute(AuthenticationBuilder parameter)
	{
		var settings = parameter.Services.Deferred<DiscordIdentitySettings>();
		parameter.AddDiscord(new ConfigureAuthentication(settings, _configure).Execute);
		parameter.Services.Register<DiscordIdentitySettings>();
	}
}