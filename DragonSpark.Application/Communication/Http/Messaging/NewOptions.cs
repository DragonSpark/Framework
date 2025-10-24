using System.Text.Json;
using DragonSpark.Model.Selection;
using Refit;

namespace DragonSpark.Application.Communication.Http.Messaging;

public sealed class NewOptions : ISelect<JsonNamingPolicy?, JsonSerializerOptions>
{
    public static NewOptions Default { get; } = new();

    NewOptions() {}

    public JsonSerializerOptions Get(JsonNamingPolicy? parameter)
    {
        var result = SystemTextJsonContentSerializer.GetDefaultJsonSerializerOptions();
        result.PropertyNamingPolicy = parameter;
        result.Converters.Remove(result.Converters[^1]);
        return result;
    }
}