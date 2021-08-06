namespace DragonSpark.Application.Entities.Queries.Materialization
{
	public sealed class Materializers<T>
	{
		public static Materializers<T> Default { get; } = new Materializers<T>();

		Materializers() : this(DefaultToList<T>.Default, DefaultToArray<T>.Default) {}

		public Materializers(IToList<T> toList, IToArray<T> toArray)
		{
			ToList  = toList;
			ToArray = toArray;
		}

		public IToList<T> ToList { get; }

		public IToArray<T> ToArray { get; }
	}
}