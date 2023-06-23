using AspNet.Security.OAuth.Mixcloud;
using DragonSpark.Model.Operations.Selection;
using Flurl;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DragonSpark.Identity.Mixcloud.Api;

sealed class UserIdentifierResponse : ISelecting<string, HttpResponseMessage>
{
	readonly IHttpClientFactory _clients;
	readonly Uri                _root;

	public UserIdentifierResponse(IHttpClientFactory clients)
		: this(clients, MixcloudAuthenticationDefaults.UserInformationEndpoint) {}

	public UserIdentifierResponse(IHttpClientFactory clients, string endpoint)
		: this(clients, new Uri(new Uri(endpoint).GetLeftPart(UriPartial.Authority))) {}

	public UserIdentifierResponse(IHttpClientFactory clients, Uri root)
	{
		_clients = clients;
		_root    = root;
	}

	public async ValueTask<HttpResponseMessage> Get(string parameter)
	{
		using var client   = _clients.CreateClient();
		var       location = _root.AppendPathSegment(parameter);
		var       result   = await client.GetAsync(location);
		return result;
	}
}