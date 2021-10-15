namespace DragonSpark.Application.Entities.Queries.Runtime.Materialize;

public sealed class Sequences<T>
{
	public static Sequences<T> Default { get; } = new Sequences<T>();

	Sequences() : this(DefaultToList<T>.Default, DefaultToArray<T>.Default) {}

	public Sequences(IToList<T> toList, IToArray<T> toArray)
	{
		ToList  = toList;
		ToArray = toArray;
	}

	public IToList<T> ToList { get; }

	public IToArray<T> ToArray { get; }
}