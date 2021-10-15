using DragonSpark.Model.Commands;

namespace DragonSpark.Model.Sequences.Collections.Groups;

sealed class GroupingAwareCommand<T> : ICommand<IGroupCollectionAware<T>>
{
	readonly IGroupCollection<T> _collection;

	public GroupingAwareCommand(IGroupCollection<T> collection) => _collection = collection;

	public void Execute(IGroupCollectionAware<T> parameter)
	{
		parameter.Execute(_collection);
	}
}