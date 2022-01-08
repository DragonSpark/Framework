namespace DragonSpark.Composition.Compose;

public sealed class DefaultServiceComponentLocator<T> : ServiceComponentLocator<T>
{
	public static DefaultServiceComponentLocator<T> Default { get; } = new();

	DefaultServiceComponentLocator() {}
}