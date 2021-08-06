namespace DragonSpark.Application.Entities.Queries
{
	public interface ICount<in T> : IMaterializer<T, uint> {}
}