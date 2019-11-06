using DragonSpark.Compose;

namespace DragonSpark.Runtime.Environment
{
	public sealed class DefaultComponent<T> : Component<T>
	{
		public static implicit operator T(DefaultComponent<T> result) => result.Get();

		public static DefaultComponent<T> Default { get; } = new DefaultComponent<T>();

		DefaultComponent() : base(Start.A.Result<T>().By.Default()) {}
	}
}