namespace DragonSpark.Application.Entities.Queries
{
	public interface ILargeCount<in T> : IQuerying<T, ulong> {}
}