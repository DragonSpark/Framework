using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Identity.Facebook;

sealed class ConfigureApplication : ICommand<AuthenticationBuilder>
{
	readonly Action<FacebookOptions> _configure;

	public ConfigureApplication(Action<FacebookOptions> configure) => _configure = configure;

	public void Execute(AuthenticationBuilder parameter)
	{
		parameter.AddFacebook()
		         .Services.Register<FacebookApplicationSettings>()
		         .AddOptions<FacebookOptions>(FacebookDefaults.AuthenticationScheme)
		         .Configure<FacebookApplicationSettings>((options, settings) =>
		                                                 {
			                                                 options.ClientId     = settings.Key;
			                                                 options.ClientSecret = settings.Secret;

			                                                 _configure(options);
		                                                 });
	}
}