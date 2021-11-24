using DragonSpark.Model.Selection;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace DragonSpark.Identity.DeviantArt.Api;

sealed class GetAccessToken : IAccessToken
{
	readonly IHttpClientFactory                        _clients;
	readonly Uri                                       _location;
	readonly ISelect<AccessTokenResponse, AccessToken> _token;

	public GetAccessToken(IHttpClientFactory clients, AccessTokenLocation location)
		: this(clients, location, AccessTokens.Default) {}

	public GetAccessToken(IHttpClientFactory clients, Uri location, ISelect<AccessTokenResponse, AccessToken> token)
	{
		_clients  = clients;
		_location = location;
		_token    = token;
	}

	public async ValueTask<AccessToken> Get()
	{
		using var client = _clients.CreateClient();
		var response = await client.GetFromJsonAsync<AccessTokenResponse>(_location).ConfigureAwait(false)
		               ??
		               throw new InvalidOperationException();
		var result = _token.Get(response);
		return result;
	}
}