namespace DragonSpark.Application.Entities.Queries
{
	public sealed class Materialize<T>
	{
		public static Materialize<T> Default { get; } = new Materialize<T>();

		Materialize() : this(DefaultToList<T>.Default, DefaultToArray<T>.Default) {}

		public Materialize(IToList<T> toList, IToArray<T> toArray)
		{
			ToList  = toList;
			ToArray = toArray;
		}

		public IToList<T> ToList { get; }

		public IToArray<T> ToArray { get; }
	}
}