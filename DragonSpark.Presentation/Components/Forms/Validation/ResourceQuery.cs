using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Forms.Validation;

sealed class ResourceQuery : IResourceQuery
{
	readonly IHttpClientFactory _clients;
	readonly TimeSpan           _timeout;

	public ResourceQuery(IHttpClientFactory clients) : this(clients, TimeSpan.FromMilliseconds(7500)) {}

	public ResourceQuery(IHttpClientFactory clients, TimeSpan timeout)
	{
		_clients = clients;
		_timeout = timeout;
	}

	/// <summary>
	/// ATTRIBUTION: https://stackoverflow.com/questions/1979915/can-i-check-if-a-file-exists-at-a-url/12013240
	/// </summary>
	/// <param name="parameter"></param>
	/// <returns></returns>
	public async ValueTask<ResourceQueryRecord?> Get(Stop<string> parameter)
	{
		using var client = _clients.CreateClient();
		client.Timeout = _timeout;
		try
		{
			var response = await client.SendAsync(new(HttpMethod.Head, parameter), parameter).Off();
			return response.IsSuccessStatusCode
				       ? new ResourceQueryRecord(parameter, response.StatusCode, response.Content.Headers.ContentType)
				       : null;
		}
		catch (UriFormatException)
		{
			return null;
		}
	}
}