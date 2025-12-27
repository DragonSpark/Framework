using DragonSpark.Model.Selection;
using Refit;
using System.Text.Json;

namespace DragonSpark.Application.Communication.Http.Messaging;

public sealed class NewSerializer : ISelect<JsonSerializerOptions, IHttpContentSerializer>
{
	public static NewSerializer Default { get; } = new();

	NewSerializer() {}

	public IHttpContentSerializer Get(JsonSerializerOptions parameter)
		=> new PolymorphicAwareContentSerializer(parameter);
}