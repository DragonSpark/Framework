using AspNet.Security.OAuth.Amazon;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Identity.Amazon;

sealed class ConfigureApplication : ICommand<AuthenticationBuilder>
{
	readonly Action<AmazonAuthenticationOptions> _configure;

	public ConfigureApplication(Action<AmazonAuthenticationOptions> configure) => _configure = configure;

	public void Execute(AuthenticationBuilder parameter)
	{
		parameter.AddAmazon()
		         .Services.Register<AmazonApplicationSettings>()
		         .AddOptions<AmazonAuthenticationOptions>(AmazonAuthenticationDefaults.AuthenticationScheme)
		         .Configure<AmazonApplicationSettings>((options, settings) =>
		                                               {
			                                               options.ClientId     = settings.Key;
			                                               options.ClientSecret = settings.Secret;

			                                               _configure(options);
		                                               });
	}
}