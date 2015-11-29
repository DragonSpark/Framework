namespace DragonSpark.Runtime.Values
{
	public interface IWritableValue<T> : IValue<T>
	{
		void Assign( T item );
	}
}