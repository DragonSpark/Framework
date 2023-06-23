using DragonSpark.Compose;
using DragonSpark.Model.Operations.Selection;
using Humanizer;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DragonSpark.Identity.DeviantArt.Api;

sealed class UserIdentifierResponse : ISelecting<string, HttpResponseMessage>
{
	readonly IHttpClientFactory _clients;
	readonly IAccessToken       _token;
	readonly string             _template;

	public UserIdentifierResponse(IHttpClientFactory clients, IAccessToken token)
		: this(clients, token, "https://www.deviantart.com/api/v1/oauth2/user/profile/{0}?access_token={1}") {}

	public UserIdentifierResponse(IHttpClientFactory clients, IAccessToken token, string template)
	{
		_clients  = clients;
		_token    = token;
		_template = template;
	}

	public async ValueTask<HttpResponseMessage> Get(string parameter)
	{
		var       token    = await _token.Await();
		using var client   = _clients.CreateClient();
		var       location = new Uri(_template.FormatWith(parameter, token.Token));
		var       result   = await client.GetAsync(location);
		return result;
	}
}