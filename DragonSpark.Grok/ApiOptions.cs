using DragonSpark.Application.Communication.Http.Messaging;
using DragonSpark.Compose;
using DragonSpark.Model.Results;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DragonSpark.Grok;

public sealed class ApiOptions : Instance<JsonSerializerOptions>
{
	public static ApiOptions Default { get; } = new();

	ApiOptions() : base(NewOptions.Default.Get(JsonNamingPolicy.SnakeCaseLower)
	                              .With(x =>
	                                    {
		                                    x.DefaultIgnoreCondition      = JsonIgnoreCondition.WhenWritingNull;
		                                    x.PropertyNameCaseInsensitive = true;
	                                    })) {}
}