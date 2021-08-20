namespace DragonSpark.Application.Entities.Queries.Materialize
{
	public interface ILargeCount<in T> : IMaterializer<T, ulong> {}
}