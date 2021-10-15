using DragonSpark.Runtime.Activation;

namespace DragonSpark.Compose.Extents;

public sealed class Instance<T>
{
	public static Instance<T> Implementation { get; } = new Instance<T>();

	Instance() {}

	public T Default => default!;

	public T New() => Location<T>.Default.New();

	public T Activate() => Location<T>.Default.Activate();

	public T Singleton() => Singleton<T>.Default.Get();
}