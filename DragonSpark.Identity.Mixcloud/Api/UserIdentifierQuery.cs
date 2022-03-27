using DragonSpark.Compose;
using DragonSpark.Diagnostics.Logging;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace DragonSpark.Identity.Mixcloud.Api;

sealed class UserIdentifierQuery : IUserIdentifierQuery
{
	readonly UserIdentifierResponse _response;
	readonly Template               _template;

	public UserIdentifierQuery(UserIdentifierResponse response, Template template)
	{
		_response = response;
		_template = template;
	}

	public async ValueTask<string?> Get(string parameter)
	{
		var response = await _response.Await(parameter);

		if (response.IsSuccessStatusCode)
		{
			return parameter;
		}

		var body = await response.Content.ReadFromJsonAsync<ErrorResponse>() ?? throw new InvalidOperationException();
		_template.Execute(parameter, body.Error.Type, body.Error.Message);

		return null;
	}

	public sealed class Template : LogWarning<string, string?, string?>
	{
		public Template(ILogger<Template> logger)
			: base(logger, "There was a problem with querying {Query}: {Code} - {Message}") {}
	}
}