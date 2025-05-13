using DragonSpark.Application.Security.Identity.Bearer;
using System.Net.Http;

namespace DragonSpark.Application.Communication.Http;

public sealed class AuthenticatedClients : UserClients
{
	public AuthenticatedClients(ApplicationUserAwareBearer bearer, IHttpClientFactory clients)
		: base(bearer, clients, "Authenticated") {}
}