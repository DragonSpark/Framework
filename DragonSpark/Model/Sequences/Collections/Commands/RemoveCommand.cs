using DragonSpark.Model.Commands;
using System.Collections.Generic;

namespace DragonSpark.Model.Sequences.Collections.Commands;

class RemoveCommand<T> : ICommand<T>
{
	readonly ICollection<T> _collection;

	public RemoveCommand(ICollection<T> collection) => _collection = collection;

	public void Execute(T parameter)
	{
		_collection.Remove(parameter);
	}
}