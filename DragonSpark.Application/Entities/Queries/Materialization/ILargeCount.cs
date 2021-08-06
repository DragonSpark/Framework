namespace DragonSpark.Application.Entities.Queries.Materialization
{
	public interface ILargeCount<in T> : IMaterializer<T, ulong> {}
}