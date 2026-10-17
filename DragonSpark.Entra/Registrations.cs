using DragonSpark.Application.AspNet.Security.Identity.Profile;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Entra;

sealed class Registrations : ICommand<AuthenticationBuilder>
{
	public static Registrations Default { get; } = new();

	Registrations() {}

	public void Execute(AuthenticationBuilder parameter)
	{
		parameter.Services.Register<EntraApplicationSettings>()
		         .AddOptions<OpenIdConnectOptions>("Entra")
		         .Configure<EntraApplicationSettings>((to, from) =>
		                                              {
			                                              to.Authority    = $"{from.Instance}{from.TenantId}/v2.0";
			                                              to.ClientId     = from.ClientId;
			                                              to.ClientSecret = from.ClientSecret;
			                                              to.CallbackPath = from.CallbackPath;
			                                              to.ResponseType = from.ResponseType;
		                                              });

		parameter.AddOpenIdConnect("Entra", "Microsoft Entra ID", x =>
		                                                          {
			                                                          x.SignInScheme = IdentityConstants.ExternalScheme;
			                                                          x.SaveTokens   = true;
		                                                          });

		parameter.AddScheme<AuthenticationSchemeOptions, EasyAuthHandler>("EasyAuth", null);

		parameter.AddPolicyScheme("SmartAuthenticate", null,
		                          o =>
		                          {
			                          o.ForwardDefaultSelector =
				                          c => c.Request.Headers.ContainsKey("X-MS-CLIENT-PRINCIPAL")
					                               ? "EasyAuth"
					                               : IdentityConstants.ApplicationScheme;
		                          });

		parameter.AddPolicyScheme("SmartChallenge", null,
		                          o =>
		                          {
			                          o.ForwardDefaultSelector =
				                          c => c.Request.Headers.ContainsKey("X-MS-CLIENT-PRINCIPAL")
					                               ? "EasyAuth"
					                               : "Entra";
		                          });

		parameter.Services.Configure<AuthenticationOptions>(o =>
		                                                    {
			                                                    o.DefaultAuthenticateScheme = "SmartAuthenticate";
			                                                    o.DefaultChallengeScheme    = "SmartChallenge";
			                                                    o.DefaultScheme             = "SmartAuthenticate";
		                                                    });

		parameter.Services.TryDecorate(typeof(ILocateUser<>), typeof(EntraAwareLocateUser<>));
	}
}