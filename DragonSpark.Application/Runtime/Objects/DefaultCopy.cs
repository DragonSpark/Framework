namespace DragonSpark.Application.Runtime.Objects;

public sealed class DefaultCopy<T> : Copy<T> where T : notnull
{
	public static DefaultCopy<T> Default { get; } = new();

	DefaultCopy() : base(DefaultSerializer<T>.Default) {}
}