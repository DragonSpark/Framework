namespace DragonSpark.Runtime.Environment
{
	sealed class DefaultComponentLocator<T> : SystemStore<T>
	{
		public static DefaultComponentLocator<T> Default { get; } = new DefaultComponentLocator<T>();

		DefaultComponentLocator() : base(LocateGuard<T>.Default.Then().Out().In(ComponentLocator<T>.Default)) {}

		public static implicit operator T(DefaultComponentLocator<T> instance) => instance.Get();
	}
}