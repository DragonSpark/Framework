using System.Text.Json;

namespace DragonSpark.Application.Runtime.Objects;

public sealed class DefaultSerializer<T> : Serializer<T> where T : notnull
{
	public static DefaultSerializer<T> Default { get; } = new();

	DefaultSerializer() : base(JsonSerializerOptions.Default) {}
}