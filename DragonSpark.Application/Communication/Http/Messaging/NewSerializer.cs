using System.Text.Json;
using DragonSpark.Model.Selection;
using Refit;

namespace DragonSpark.Application.Communication.Http.Messaging;

public sealed class NewSerializer : ISelect<JsonSerializerOptions, IHttpContentSerializer>
{
    public static NewSerializer Default { get; } = new();

    NewSerializer() {}

    public IHttpContentSerializer Get(JsonSerializerOptions parameter) => new NoContentAwareSerializer(parameter);
}