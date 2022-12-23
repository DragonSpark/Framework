using AspNet.Security.OAuth.DeviantArt;
using DragonSpark.Application.Security.Identity.Claims.Actions;
using DragonSpark.Composition;
using DragonSpark.Identity.DeviantArt.Claims;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;

namespace DragonSpark.Identity.DeviantArt;

sealed class ConfigureApplication : ICommand<AuthenticationBuilder>
{
	public static ConfigureApplication Default { get; } = new ConfigureApplication();

	ConfigureApplication() : this(DefaultClaimActions.Default, _ => {}) {}

	readonly IClaimAction                            _claims;
	readonly Action<DeviantArtAuthenticationOptions> _configure;

	public ConfigureApplication(Action<DeviantArtAuthenticationOptions> configure)
		: this(DefaultClaimActions.Default, configure) {}

	public ConfigureApplication(IClaimAction claims, Action<DeviantArtAuthenticationOptions> configure)
	{
		_claims    = claims;
		_configure = configure;
	}

	public void Execute(AuthenticationBuilder parameter)
	{
		var settings = parameter.Services.Deferred<DeviantArtApplicationSettings>();
		parameter
			.AddOAuth<DeviantArtAuthenticationOptions,
				DeviantArtAuthenticationHandler>(DeviantArtAuthenticationDefaults.AuthenticationScheme,
				                                 DeviantArtAuthenticationDefaults.DisplayName,
				                                 new ConfigureAuthentication(settings, _claims, _configure).Execute);
		parameter.Services.Register<DeviantArtApplicationSettings>();
	}
}

sealed class DeviantArtAuthenticationHandler : AspNet.Security.OAuth.DeviantArt.DeviantArtAuthenticationHandler
{
	public DeviantArtAuthenticationHandler(IOptionsMonitor<DeviantArtAuthenticationOptions> options,
	                                       ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
		: base(options, logger, encoder, clock) {}

	protected override async Task<OAuthTokenResponse> ExchangeCodeAsync(OAuthCodeExchangeContext context)
	{
		var tokenRequestParameters = new Dictionary<string, string>()
		{
			{ "client_id", Options.ClientId },
			{ "redirect_uri", context.RedirectUri },
			{ "client_secret", Options.ClientSecret },
			{ "code", context.Code },
			{ "grant_type", "authorization_code" },
		};

		// PKCE https://tools.ietf.org/html/rfc7636#section-4.5, see BuildChallengeUrl
		if (context.Properties.Items.TryGetValue(OAuthConstants.CodeVerifierKey, out var codeVerifier))
		{
			tokenRequestParameters.Add(OAuthConstants.CodeVerifierKey, codeVerifier!);
			context.Properties.Items.Remove(OAuthConstants.CodeVerifierKey);
		}

		var requestContent = new FormUrlEncodedContent(tokenRequestParameters!);

		var requestMessage = new HttpRequestMessage(HttpMethod.Post, Options.TokenEndpoint);
		requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
		requestMessage.Content = requestContent;
		requestMessage.Version = Backchannel.DefaultRequestVersion;
		var response = await Backchannel.SendAsync(requestMessage, Context.RequestAborted);
		var body     = await response.Content.ReadAsStringAsync(Context.RequestAborted);

		return response.IsSuccessStatusCode switch
		{
			true => OAuthTokenResponse.Success(JsonDocument.Parse(body)),
			false => PrepareFailedOAuthTokenReponse(response, body)
		};
	}

	OAuthTokenResponse PrepareFailedOAuthTokenReponse(HttpResponseMessage response, string body)
	{
		try
		{
			var exception = GetStandardErrorException(JsonDocument.Parse(body));

			if (exception is null)
			{
				var errorMessage = $"OAuth token endpoint failure: Status: {response.StatusCode};Headers: {response.Headers};Body: {body};";
				return OAuthTokenResponse.Failed(new Exception(errorMessage));
			}

			return OAuthTokenResponse.Failed(exception);
		}
		catch (JsonException e)
		{
			Logger.LogWarning(e, "Could not parse {Body}", body);
			throw;
		}
	}

	static Exception? GetStandardErrorException(JsonDocument response)
	{
		var root  = response.RootElement;
		var error = root.GetString("error");

		if (error is not null)
		{
			var result = new StringBuilder("OAuth token endpoint failure: ");
			result.Append(error);

			if (root.TryGetProperty("error_description", out var errorDescription))
			{
				result.Append(";Description=");
				result.Append(errorDescription);
			}

			if (root.TryGetProperty("error_uri", out var errorUri))
			{
				result.Append(";Uri=");
				result.Append(errorUri);
			}

			return new(result.ToString())
			{
				Data =
				{
					["error"]             = error,
					["error_description"] = errorDescription.ToString(),
					["error_uri"]         = errorUri.ToString()
				}
			};
		}

		return null;
	}


}