namespace DragonSpark.Reflection.Members
{
	public sealed class DefaultPropertyDelegate<T, TProperty> : PropertyDelegate<T, TProperty>
	{
		public static DefaultPropertyDelegate<T, TProperty> Default { get; }
			= new DefaultPropertyDelegate<T, TProperty>();

		DefaultPropertyDelegate() {}
	}
}