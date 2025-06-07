using System.Text.Json;

namespace DragonSpark.Application.Runtime.Objects;

public sealed class DefaultDeserialize<T> : Deserialize<T>
{
    public static DefaultDeserialize<T> Default { get; } = new();

    DefaultDeserialize() {}

    public DefaultDeserialize(JsonSerializerOptions options) : base(options) {}
}