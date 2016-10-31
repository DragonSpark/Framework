using DragonSpark.Sources;

namespace DragonSpark.Runtime
{
	public interface IRepository<T> : IItemSource<T>, IComposable<T> {}
}