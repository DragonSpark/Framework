using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;
using Humanizer;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DragonSpark.Identity.DeviantArt.Api;

sealed class UserIdentifierResponse : IStopAware<string, HttpResponseMessage>
{
	readonly IHttpClientFactory _clients;
	readonly IAccessToken       _token;
	readonly string             _template;

	public UserIdentifierResponse(IHttpClientFactory clients, IAccessToken token,
	                              string template =
		                              "https://www.deviantart.com/api/v1/oauth2/user/profile/{0}?access_token={1}")
	{
		_clients  = clients;
		_token    = token;
		_template = template;
	}

	public async ValueTask<HttpResponseMessage> Get(Stop<string> parameter)
	{
		var (subject, stop) = parameter;
		var       token    = await _token.Off();
		using var client   = _clients.CreateClient();
		var       location = new Uri(_template.FormatWith(subject, token.Token));
		var       result   = await client.GetAsync(location, stop);
		return result;
	}
}