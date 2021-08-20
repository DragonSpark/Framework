namespace DragonSpark.Application.Entities.Queries.Materialize
{
	public class Materialization<T>
	{
		public Materialization(IAny<T> any, Counting<T> counting, Sequences<T> sequences)
		{
			Any         = any;
			Counting    = counting;
			Sequences = sequences;
		}

		public IAny<T> Any { get; }

		public Counting<T> Counting { get; }

		public Sequences<T> Sequences { get; }
	}
}