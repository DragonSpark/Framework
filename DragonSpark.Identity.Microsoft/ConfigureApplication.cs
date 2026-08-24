using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Identity.Microsoft;

sealed class ConfigureApplication : ICommand<AuthenticationBuilder>
{
	readonly Action<MicrosoftAccountOptions> _configure;

	public ConfigureApplication(Action<MicrosoftAccountOptions> configure) => _configure = configure;

	public void Execute(AuthenticationBuilder parameter)
	{
		parameter.AddMicrosoftAccount()
		         .Services.Register<MicrosoftApplicationSettings>()
		         .AddOptions<MicrosoftAccountOptions>(MicrosoftAccountDefaults.AuthenticationScheme)
		         .Configure<MicrosoftApplicationSettings>((to, from) =>
		                                                  {
			                                                  to.ClientId              = from.Key;
			                                                  to.ClientSecret          = from.Secret;
			                                                  to.AuthorizationEndpoint = from.AuthorizationEndpoint;
			                                                  to.TokenEndpoint         = from.TokenEndpoint;

			                                                  _configure(to);
		                                                  });
	}
}