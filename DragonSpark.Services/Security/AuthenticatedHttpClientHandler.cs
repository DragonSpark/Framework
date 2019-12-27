using DragonSpark.Model.Selection.Alterations;
using Flurl;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Services.Security
{
	sealed class AuthenticatedHttpClientHandler : Communication.HttpClientHandler
	{
		readonly string        _apiKey;
		readonly Alter<string> _secret;

		public AuthenticatedHttpClientHandler(ILogger<Communication.HttpClientHandler> logger, string apiKey,
		                                      string secret) : this(logger, apiKey, new Encryptor(secret).Get) {}

		public AuthenticatedHttpClientHandler(ILogger<Communication.HttpClientHandler> logger, string apiKey,
		                                      Alter<string> secret) : base(logger)
		{
			_apiKey = apiKey;
			_secret = secret;
		}

		protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
		                                                       CancellationToken cancellationToken)
		{
			var uri = request.RequestUri
			                 .ToString()
			                 .SetQueryParam("apikey", _apiKey)
			                 .SetQueryParam("nonce", (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds)
			                 .ToString();
			request.RequestUri = new UriBuilder(uri).Uri;
			var parameter = request.RequestUri.ToString();
			var secret    = _secret(parameter);
			request.Headers.Add("apisign", secret);

			var result = base.SendAsync(request, cancellationToken);
			return result;
		}
	}
}