namespace DragonSpark.Application.Entities.Queries
{
	public interface IAny<in T> : IQuerying<T, bool> {}
}