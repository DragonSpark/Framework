using AspNet.Security.OAuth.Discord;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Identity.Discord;

sealed class ConfigureApplication : ICommand<AuthenticationBuilder>
{
	readonly Action<DiscordAuthenticationOptions> _configure;

	public ConfigureApplication(Action<DiscordAuthenticationOptions> configure) => _configure = configure;

	public void Execute(AuthenticationBuilder parameter)
	{
		parameter.AddDiscord()
		         .Services.Register<DiscordIdentitySettings>()
		         .AddOptions<DiscordAuthenticationOptions>(DiscordAuthenticationDefaults.AuthenticationScheme)
		         .Configure<DiscordIdentitySettings>((options, settings) =>
		                                             {
			                                             options.ClientId     = settings.Key;
			                                             options.ClientSecret = settings.Secret;

			                                             _configure(options);
		                                             });
	}
}