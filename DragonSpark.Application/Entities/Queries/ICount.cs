namespace DragonSpark.Application.Entities.Queries
{
	public interface ICount<in T> : IQuerying<T, uint> {}
}