namespace DragonSpark.Runtime
{
	public class Collection : Collection<object>
	{}

	public interface IValue
	{
		object Item { get; }
	}

	public interface IValue<out T>
	{
		T Item { get; }
	}

	public interface IWritableValue<T> : IValue<T>
	{
		void Assign( T item );
	}

	public abstract class Value<T> : IValue<T>, IValue
	{
		public abstract T Item { get;  }

		object IValue.Item => Item;
	}

	public abstract class WritableValue<T> : Value<T>, IWritableValue<T>
	{
		public abstract void Assign( T item );
	}
}