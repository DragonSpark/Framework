using System.Text.Json;
using DragonSpark.Text;

namespace DragonSpark.Application.Runtime.Objects;

public class Deserialize<T> : IParser<T?>
{
    readonly JsonSerializerOptions _options;

    protected Deserialize() : this(JsonSerializerOptions.Default) {}

    protected Deserialize(JsonSerializerOptions options) => _options = options;

    public T? Get(string parameter) => JsonSerializer.Deserialize<T>(parameter, _options);
}