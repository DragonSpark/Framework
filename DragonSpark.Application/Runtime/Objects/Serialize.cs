using System.Text.Json;
using DragonSpark.Text;

namespace DragonSpark.Application.Runtime.Objects;

public class Serialize<T> : IFormatter<T>
{
    readonly JsonSerializerOptions _options;

    protected Serialize() : this(JsonSerializerOptions.Default) {}

    protected Serialize(JsonSerializerOptions options) => _options = options;

    public string Get(T parameter) => JsonSerializer.Serialize(parameter, _options);
}