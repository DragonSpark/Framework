namespace DragonSpark.Reflection.Members
{
	public sealed class DefaultPropertyDelegates<T> : PropertyDelegates<T>
	{
		public static DefaultPropertyDelegates<T> Default { get; } = new DefaultPropertyDelegates<T>();

		DefaultPropertyDelegates() {}
	}
}