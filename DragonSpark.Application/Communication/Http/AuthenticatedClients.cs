using System.Net.Http;

namespace DragonSpark.Application.Communication.Http;

public sealed class AuthenticatedClients : UserClients
{
	public AuthenticatedClients(IHttpClientFactory clients) : base(clients, "Authenticated") {}
}