using DragonSpark.Text;
using System.Text.Json;

namespace DragonSpark.Application.Runtime.Objects;

public class Deserialize<T> : IParser<T?>
{
    readonly JsonSerializerOptions _options;

    protected Deserialize() : this(JsonSerializerOptions.Default) {}

    protected Deserialize(JsonSerializerOptions options) => _options = options;

    public T? Get(string parameter)
    {
	    var deserialize = JsonSerializer.Deserialize<T>(parameter, _options);
	    return deserialize;
    }
}