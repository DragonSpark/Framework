using DragonSpark.Application.AspNet.Security.Identity;
using DragonSpark.Application.AspNet.Security.Identity.Authentication;
using DragonSpark.Application.AspNet.Security.Identity.Profile;
using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations.Selection;
using DragonSpark.Model.Results;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace DragonSpark.Entra;

sealed class Registrations<T> : ICommand<AuthenticationBuilder> where T : class
{
	public static Registrations<T> Default { get; } = new();

	Registrations() {}

	public void Execute(AuthenticationBuilder parameter)
	{
		var services = parameter.Services;
		services.Register<EntraApplicationSettings>()
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

		services.Configure<AuthenticationOptions>(o =>
		                                          {
			                                          o.DefaultAuthenticateScheme = "SmartAuthenticate";
			                                          o.DefaultChallengeScheme    = "SmartChallenge";
			                                          o.DefaultScheme             = "SmartAuthenticate";
		                                          });

		services.TryDecorate(typeof(ILocateUser<>), typeof(EntraAwareLocateUser<>));
		services.TryDecorate<IAuthenticationValidation, EntraAwareAuthenticationValidation>();
		services.Start<LocateEasyAuthUser<T>>().Singleton();
		services.AddScoped<IClaimsTransformation, EasyAuthClaimsTransformation<T>>();
	}
}

// TODO
sealed class EntraAwareAuthenticationValidation : IAuthenticationValidation
{
	readonly IAuthenticationValidation _previous;

	public EntraAwareAuthenticationValidation(IAuthenticationValidation previous) => _previous = previous;

	public ValueTask<bool> Get(ClaimsPrincipal parameter)
		=> parameter.Identity?.AuthenticationType == "EasyAuth" ? true.ToOperation() : _previous.Get(parameter);
}

sealed class MarkerClaim : Instance<Claim>
{
	public static MarkerClaim Default { get; } = new();

	MarkerClaim() : base(new("EasyAuthTransformed", "true")) {}
}

sealed class EasyAuthClaimsTransformation<T> : IClaimsTransformation where T : class
{
	readonly IUserClaimsPrincipalFactory<T> _claims;
	readonly LocateEasyAuthUser<T>          _locate;
	readonly Claim                          _marker;

	public EasyAuthClaimsTransformation(IUserClaimsPrincipalFactory<T> claims, LocateEasyAuthUser<T> locate)
		: this(claims, locate, MarkerClaim.Default) {}

	[Candidate(false)]
	public EasyAuthClaimsTransformation(IUserClaimsPrincipalFactory<T> claims, LocateEasyAuthUser<T> locate,
	                                    Claim marker)
	{
		_claims = claims;
		_locate = locate;
		_marker = marker;
	}

	public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
	{
		if (await _locate.Off(principal) is {} user
		    && await _claims.CreateAsync(user).Off() is { Identity: ClaimsIdentity identity })
		{
			identity.AddClaim(_marker);
			principal.AddIdentity(identity);
		}

		return principal;
	}
}

sealed class LocateEasyAuthUser<T> : ISelecting<ClaimsPrincipal, T?> where T : class
{
	readonly IUsers<T> _users;

	public LocateEasyAuthUser(IUsers<T> users) => _users = users;

	public async ValueTask<T?> Get(ClaimsPrincipal parameter)
	{
		if (parameter.Identity?.AuthenticationType == "EasyAuth" && !parameter.HasClaim("EasyAuthTransformed", "true"))
		{
			if ((parameter.FindFirstValue(ClaimTypes.Email) ?? parameter.FindFirstValue("preferred_username")
			     ?? parameter.FindFirstValue("email")) is {} email)
			{
				using var users = _users.Get();
				return await users.Subject.FindByEmailAsync(email).Off();
			}
		}

		return null;
	}
}