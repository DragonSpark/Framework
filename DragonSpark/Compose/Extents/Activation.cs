using DragonSpark.Runtime.Activation;

namespace DragonSpark.Compose.Extents
{
	public sealed class Activation<T>
	{
		public static Activation<T> Instance { get; } = new Activation<T>();

		Activation() {}

		public T Default => default;

		public T New() => Location<T>.Default.New();

		public T Activate() => Location<T>.Default.Activate();

		public T Locate() => Location<T>.Default.Locate();

		public T Singleton() => Singleton<T>.Default.Get();
	}
}