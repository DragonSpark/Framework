using DragonSpark.Runtime.Activation;
using DragonSpark.Runtime.Environment;

namespace DragonSpark.Compose.Extents
{
	public sealed class Location<T>
	{
		public static Location<T> Default { get; } = new Location<T>();

		Location() {}

		public T New() => New<T>.Default.Get();

		public T Activate() => Activator<T>.Default.Get();

		public T Locate() => ComponentLocator<T>.Default.Get();
	}
}