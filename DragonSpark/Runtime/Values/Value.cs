namespace DragonSpark.Runtime.Values
{
	public abstract class Value<T> : IValue<T>, IValue
	{
		public abstract T Item { get; }

		object IValue.Item => Item;
	}
}