namespace DragonSpark.Application.Runtime.Objects;

public sealed class DefaultSerialize<T> : Serialize<T>
{
    public static DefaultSerialize<T> Default { get; } = new();

    DefaultSerialize() {}
}