namespace DragonSpark.Runtime.Assignments
{
	public static class Assignments
	{
		public static Value<T> From<T>( T item ) => new Value<T>( item, item );
	}
}