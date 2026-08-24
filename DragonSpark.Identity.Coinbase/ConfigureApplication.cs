using AspNet.Security.OAuth.Coinbase;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Identity.Coinbase;

sealed class ConfigureApplication : ICommand<AuthenticationBuilder>
{
	readonly Action<CoinbaseAuthenticationOptions> _configure;

	public ConfigureApplication(Action<CoinbaseAuthenticationOptions> configure) => _configure = configure;

	public void Execute(AuthenticationBuilder parameter)
	{
		parameter.AddCoinbase()
		         .Services.Register<CoinbaseApplicationSettings>()
		         .AddOptions<CoinbaseAuthenticationOptions>(CoinbaseAuthenticationDefaults.AuthenticationScheme)
		         .Configure<CoinbaseApplicationSettings>((options, settings) =>
		                                                 {
			                                                 options.ClientId     = settings.Key;
			                                                 options.ClientSecret = settings.Secret;

			                                                 _configure(options);
		                                                 });
	}
}