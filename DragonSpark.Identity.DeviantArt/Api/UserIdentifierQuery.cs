using DragonSpark.Compose;
using DragonSpark.Diagnostics.Logging;
using DragonSpark.Model.Operations;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace DragonSpark.Identity.DeviantArt.Api;

sealed class UserIdentifierQuery : IUserIdentifierQuery
{
	readonly UserIdentifierResponse _response;
	readonly Template _template;

	public UserIdentifierQuery(UserIdentifierResponse response, Template template)
	{
		_response = response;
		_template = template;
	}

	public async ValueTask<string?> Get(Stop<string> parameter)
	{
		var response = await _response.Off(parameter);

		if (response.IsSuccessStatusCode)
		{
			var user = await response.Content.ReadFromJsonAsync<UserResponse>(parameter).Off();
			var result = user?.Result.UserId;
			return result;
		}

		var body = await response.Content.ReadFromJsonAsync<ErrorResponse>(parameter).Off() ?? throw new InvalidOperationException();
		_template.Execute(parameter, body.Error, body.ErrorMessage);

		return null;
	}

	public sealed class Template : LogWarning<string, string?, string?>
	{
		public Template(ILogger<Template> logger)
			: base(logger, "There was a problem with querying {Query}: {Code} - {Message}") { }
	}
}