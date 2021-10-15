namespace DragonSpark.Application.Entities.Queries.Runtime.Materialize;

public class Materialization<T> : IMaterialization<T>
{
	public Materialization(IMaterialization<T> previous)
		: this(previous.Any, previous.Counting, previous.Sequences) {}

	public Materialization(IAny<T> any, Counting<T> counting, Sequences<T> sequences)
	{
		Any       = any;
		Counting  = counting;
		Sequences = sequences;
	}

	public IAny<T> Any { get; }

	public Counting<T> Counting { get; }

	public Sequences<T> Sequences { get; }
}