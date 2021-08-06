namespace DragonSpark.Application.Entities.Queries
{
	public interface IAny<in T> : IMaterializer<T, bool> {}
}