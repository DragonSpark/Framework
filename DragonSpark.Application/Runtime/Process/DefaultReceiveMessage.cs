namespace DragonSpark.Application.Runtime.Process;

public sealed class DefaultReceiveMessage<T> : ReceiveMessage<T>
{
    public static DefaultReceiveMessage<T> Default { get; } = new();

    DefaultReceiveMessage() {}
}