using AspNet.Security.OAuth.Paypal;
using DragonSpark.Application.AspNet.Security.Identity.Claims.Compile;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Identity.PayPal;

sealed class ConfigureApplication : ICommand<AuthenticationBuilder>
{
	readonly Action<PaypalAuthenticationOptions> _configure;

	public ConfigureApplication(Action<PaypalAuthenticationOptions> configure) => _configure = configure;

	public void Execute(AuthenticationBuilder parameter)
	{
		var services = parameter.Services;
		services.Register<PayPalApplicationSettings>();
		services.TryDecorate<IClaims, Claims>();
		services.TryDecorate<IKnownClaims, AdditionalClaims>();
		parameter.AddPaypal()
		         .Services.Register<PayPalApplicationSettings>()
		         .AddOptions<PaypalAuthenticationOptions>(PaypalAuthenticationDefaults.AuthenticationScheme)
		         .Configure<PayPalApplicationSettings>((to, from) =>
		                                               {
			                                               to.ClientId     = from.Key;
			                                               to.ClientSecret = from.Secret;

			                                               var x = from.Authentication;
			                                               to.AuthorizationEndpoint   = x.AuthorizationEndpoint;
			                                               to.TokenEndpoint           = x.TokenEndpoint;
			                                               to.UserInformationEndpoint = x.UserInformationEndpoint;

			                                               ClaimActions.Default.Execute(to.ClaimActions);

			                                               if (x.Scopes is not null)
			                                               {
				                                               to.Scope.Clear();
				                                               foreach (var scope in x.Scopes)
				                                               {
					                                               to.Scope.Add(scope);
				                                               }
			                                               }

			                                               _configure(to);
		                                               });
	}
}