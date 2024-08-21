using DragonSpark.Application.Communication;
using DragonSpark.Model.Results;
using System.Net.Http;

namespace DragonSpark.Presentation.Environment;

public sealed class PropagatedClient : IResult<HttpClient>
{
	readonly IHttpClientFactory _clients;
	readonly CurrentCookie      _cookie;
	readonly string             _name;

	public PropagatedClient(IHttpClientFactory clients, CurrentCookie cookie)
		: this(clients, cookie,
		       CookieHeaderName.Default) {}

	public PropagatedClient(IHttpClientFactory clients, CurrentCookie cookie, string name)
	{
		_clients = clients;
		_cookie  = cookie;
		_name    = name;
	}

	public HttpClient Get()
	{
		var result = _clients.CreateClient();
		result.DefaultRequestHeaders.Add(_name, _cookie.Get());
		return result;
	}
}