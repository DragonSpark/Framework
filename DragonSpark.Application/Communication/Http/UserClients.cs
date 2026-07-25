using DragonSpark.Model.Results;

namespace DragonSpark.Application.Communication.Http;

public abstract class UserClients : Result<HttpClient>
{
	protected UserClients(IHttpClientFactory clients, string name) : base(() => clients.CreateClient(name)) {}
}