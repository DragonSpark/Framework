using DragonSpark.Model.Commands;

namespace DragonSpark.Model.Sequences.Collections.Groups
{
	public interface IGroupCollectionAware<T> : ICommand<IGroupCollection<T>> {}
}