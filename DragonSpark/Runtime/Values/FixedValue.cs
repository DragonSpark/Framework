namespace DragonSpark.Runtime.Values
{
	public class FixedValue<T> : WritableValue<T>
	{
		T reference;

		public override void Assign( T item )
		{
			reference = item;
		}

		public override T Item => reference;
	}
}