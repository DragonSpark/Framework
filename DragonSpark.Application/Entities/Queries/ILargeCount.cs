namespace DragonSpark.Application.Entities.Queries
{
	public interface ILargeCount<in T> : IMaterializer<T, ulong> {}
}