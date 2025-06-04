using DragonSpark.Text;
using System.Text.Json;

namespace DragonSpark.Application.Runtime.Objects;

public class Serialize<T> : IFormatter<T>
{
    readonly JsonSerializerOptions _options;

    protected Serialize() : this(JsonSerializerOptions.Default) {}

    public Serialize(JsonSerializerOptions options) => _options = options;

    public string Get(T parameter) => JsonSerializer.Serialize(parameter, _options);
}