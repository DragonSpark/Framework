using AspNet.Security.OAuth.Patreon;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Identity.Patreon;

sealed class ConfigureApplication : ICommand<AuthenticationBuilder>
{
	readonly Action<PatreonAuthenticationOptions> _configure;

	public ConfigureApplication(Action<PatreonAuthenticationOptions> configure) => _configure = configure;

	public void Execute(AuthenticationBuilder parameter)
	{
		parameter.AddPatreon()
		         .Services.Register<PatreonApplicationSettings>()
		         .AddOptions<PatreonAuthenticationOptions>(PatreonAuthenticationDefaults.AuthenticationScheme)
		         .Configure<PatreonApplicationSettings>((options, settings) =>
		                                                {
			                                                options.ClientId     = settings.Key;
			                                                options.ClientSecret = settings.Secret;

			                                                _configure(options);
		                                                });
	}
}