using DragonSpark.Application.Components.Validation.Expressions;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Forms.Validation;

public sealed class ResourceExistsValidation : IValidatingValue<string>
{
	readonly IHttpClientFactory _clients;
	readonly TimeSpan           _timeout;

	public ResourceExistsValidation(IHttpClientFactory clients) : this(clients, TimeSpan.FromMilliseconds(7500)) {}

	public ResourceExistsValidation(IHttpClientFactory clients, TimeSpan timeout)
	{
		_clients = clients;
		_timeout = timeout;
	}

	/// <summary>
	/// ATTRIBUTION: https://stackoverflow.com/questions/1979915/can-i-check-if-a-file-exists-at-a-url/12013240
	/// </summary>
	/// <param name="parameter"></param>
	/// <returns></returns>
	public async ValueTask<bool> Get(string parameter)
	{
		using var client = _clients.CreateClient();
		client.Timeout = _timeout;
		try
		{
			var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Head, parameter))
			                           .ConfigureAwait(false);
			return response.IsSuccessStatusCode;
		}
		catch (UriFormatException)
		{
			return false;
		}
	}
}