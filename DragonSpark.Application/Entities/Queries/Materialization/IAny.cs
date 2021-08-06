namespace DragonSpark.Application.Entities.Queries.Materialization
{
	public interface IAny<in T> : IMaterializer<T, bool> {}
}