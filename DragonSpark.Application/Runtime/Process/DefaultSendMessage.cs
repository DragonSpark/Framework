namespace DragonSpark.Application.Runtime.Process;

/// <summary>
/// ATTRIBUTION: https://stackoverflow.com/a/60546519
/// </summary>
public sealed class DefaultSendMessage<T> : SendMessage<T>
{
    public static DefaultSendMessage<T> Default { get; } = new();

    DefaultSendMessage() {}
}