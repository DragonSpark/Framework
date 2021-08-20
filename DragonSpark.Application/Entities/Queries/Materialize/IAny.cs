namespace DragonSpark.Application.Entities.Queries.Materialize
{
	public interface IAny<in T> : IMaterializer<T, bool> {}
}