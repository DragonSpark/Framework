namespace DragonSpark.Application.Entities.Queries.Materialize
{
	public interface IMaterialization<T>
	{
		IAny<T> Any { get; }
		Counting<T> Counting { get; }
		Sequences<T> Sequences { get; }
	}
}