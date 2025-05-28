using DragonSpark.Model.Results;
using System.Net.Http;

namespace DragonSpark.Application.Communication.Http;

public abstract class UserClients : Result<HttpClient>
{
	protected UserClients(IHttpClientFactory clients, string name) : base(() => clients.CreateClient(name)) {}
}