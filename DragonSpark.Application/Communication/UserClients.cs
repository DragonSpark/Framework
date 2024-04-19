using DragonSpark.Application.Security.Identity.Bearer;
using System.Net.Http;

namespace DragonSpark.Application.Communication;

public abstract class UserClients : User<HttpClient>
{
	protected UserClients(ApplicationUserAwareBearer bearer, IHttpClientFactory clients, string name)
		: base(bearer, () => clients.CreateClient(name)) {}
}