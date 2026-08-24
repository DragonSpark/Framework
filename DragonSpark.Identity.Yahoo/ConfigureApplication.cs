using AspNet.Security.OAuth.Yahoo;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Identity.Yahoo;

sealed class ConfigureApplication : ICommand<AuthenticationBuilder>
{
	readonly Action<YahooAuthenticationOptions> _configure;

	public ConfigureApplication(Action<YahooAuthenticationOptions> configure) => _configure = configure;

	public void Execute(AuthenticationBuilder parameter)
	{
		parameter.AddYahoo()
		         .Services.Register<YahooApplicationSettings>()
		         .AddOptions<YahooAuthenticationOptions>(YahooAuthenticationDefaults.AuthenticationScheme)
		         .Configure<YahooApplicationSettings>((options, settings) =>
		                                              {
			                                              options.ClientId     = settings.Key;
			                                              options.ClientSecret = settings.Secret;

			                                              _configure(options);
		                                              });
	}
}