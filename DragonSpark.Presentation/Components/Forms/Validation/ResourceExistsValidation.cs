using DragonSpark.Application.Components.Validation.Expressions;
using Microsoft.AspNetCore.Components;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Forms.Validation
{
	public sealed class ResourceExistsValidation : ComponentBase, IValidatingValue<string>
	{
		readonly TimeSpan _timeout;

		public ResourceExistsValidation() : this(TimeSpan.FromMilliseconds(2500)) {}

		public ResourceExistsValidation(TimeSpan timeout) => _timeout = timeout;

		[Inject]
		IHttpClientFactory Clients { get; set; } = default!;

		/// <summary>
		/// ATTRIBUTION: https://stackoverflow.com/questions/1979915/can-i-check-if-a-file-exists-at-a-url/12013240
		/// </summary>
		/// <param name="parameter"></param>
		/// <returns></returns>
		public async ValueTask<bool> Get(string parameter)
		{
			if (!string.IsNullOrEmpty(parameter))
			{
				using var client  = Clients.CreateClient();
				client.Timeout = _timeout;
				var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Head, parameter));
				return response.IsSuccessStatusCode;
			}

			return true;
		}
	}
}