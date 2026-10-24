using System.Text.Json;

namespace DragonSpark.Application.Runtime.Objects;

public sealed class DefaultSerializer<T> : Serializer<T>
{
	public static DefaultSerializer<T> Default { get; } = new();

	DefaultSerializer() : base(JsonSerializerOptions.Default) {}
}