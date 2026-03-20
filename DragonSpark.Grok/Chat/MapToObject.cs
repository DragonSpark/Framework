using System.Collections.Generic;
using System.Text.Json;
using DragonSpark.Compose;
using DragonSpark.Model.Selection;

namespace DragonSpark.Grok.Chat;

sealed class MapToObject<T> : ISelect<IReadOnlyDictionary<string, object>, T>
{
    public static MapToObject<T> Default { get; } = new();

    MapToObject() : this(new()
    {
        PropertyNameCaseInsensitive = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    }) {}

    readonly JsonSerializerOptions _options;

    public MapToObject(JsonSerializerOptions options) => _options = options;

    public T Get(IReadOnlyDictionary<string, object> parameter)
    {
        var content = JsonSerializer.Serialize(parameter);
        return JsonSerializer.Deserialize<T>(content, _options).Verify();
    }
}