using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace DragonSpark.Entra;

sealed class EasyAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
	public EasyAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger,
	                       UrlEncoder encoder)
		: base(options, logger, encoder) {}

	protected override Task<AuthenticateResult> HandleAuthenticateAsync()
	{
		if (Request.Headers.TryGetValue("X-MS-CLIENT-PRINCIPAL", out var header))
		{
			try
			{
				var decoded = Convert.FromBase64String(header.ToString());
				var json    = JsonSerializer.Deserialize<JsonElement>(decoded);
				var claims = json.GetProperty("claims")
				                 .EnumerateArray()
				                 .Select(c => new Claim(c.GetProperty("typ").GetString()!,
				                                        c.GetProperty("val").GetString()!));

				var authType = json.GetProperty("auth_typ").GetString();
				var nameType = json.GetProperty("name_typ").GetString();
				var roleType = json.GetProperty("role_typ").GetString();
				var identity = new ClaimsIdentity(claims, authType, nameType, roleType);
				var ticket   = new AuthenticationTicket(new(identity), Scheme.Name);

				return Task.FromResult(AuthenticateResult.Success(ticket));
			}
			catch (Exception ex)
			{
				Logger.LogError(ex, "Failed to parse Azure Easy Auth header");
				return Task.FromResult(AuthenticateResult.Fail(ex));
			}
		}

		return Task.FromResult(AuthenticateResult.NoResult());
	}
}