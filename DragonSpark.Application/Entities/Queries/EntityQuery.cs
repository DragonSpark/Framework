namespace DragonSpark.Application.Entities.Queries
{
	public class EntityQuery<T>
	{
		public EntityQuery(IAny<T> any, Counting<T> counting, Materialize<T> materialize)
		{
			Any         = any;
			Counting    = counting;
			Materialize = materialize;
		}

		public IAny<T> Any { get; }

		public Counting<T> Counting { get; }

		public Materialize<T> Materialize { get; }
	}
}