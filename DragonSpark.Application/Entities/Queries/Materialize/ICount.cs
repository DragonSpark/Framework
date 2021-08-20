namespace DragonSpark.Application.Entities.Queries.Materialize
{
	public interface ICount<in T> : IMaterializer<T, uint> {}
}