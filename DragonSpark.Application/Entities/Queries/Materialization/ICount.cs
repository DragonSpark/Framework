namespace DragonSpark.Application.Entities.Queries.Materialization
{
	public interface ICount<in T> : IMaterializer<T, uint> {}
}