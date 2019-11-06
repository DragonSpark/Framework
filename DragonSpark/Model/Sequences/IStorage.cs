using DragonSpark.Model.Commands;

namespace DragonSpark.Model.Sequences
{
	public interface IStorage<T> : IStores<T>, ICommand<T[]> {}
}