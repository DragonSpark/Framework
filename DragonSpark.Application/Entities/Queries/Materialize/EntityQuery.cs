namespace DragonSpark.Application.Entities.Queries.Materialize
{
	public class EntityQuery<T>
	{
		public EntityQuery(IAny<T> any, Counting<T> counting, Materializers<T> materializers)
		{
			Any         = any;
			Counting    = counting;
			Materializers = materializers;
		}

		public IAny<T> Any { get; }

		public Counting<T> Counting { get; }

		public Materializers<T> Materializers { get; }
	}
}