namespace DragonSpark.Runtime.Values
{
	public abstract class WritableValue<T> : Value<T>, IWritableValue<T>
	{
		public abstract void Assign( T item );
	}
}