using DragonSpark.Application.Entities.Editing;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.SyncfusionRendering.Entities;

public class EntityState<T> : IOperation<Updated<T>> where T : class
{
	readonly Await<T>  _add;
	readonly Update<T> _update;
	readonly Remove<T> _remove;

	protected EntityState(IOperation<T> add, Update<T> update, Remove<T> remove) : this(add.Await, update, remove) {}

	protected EntityState(Await<T> add, Update<T> update, Remove<T> remove)
	{
		_add    = add;
		_update = update;
		_remove = remove;
	}

	public async ValueTask Get(Updated<T> parameter)
	{
		var (subject, action) = parameter;
		switch (action)
		{
			case "Add":
				await _add(subject);
				await _update.Await(subject);
				break;
			case "Edit":
				await _update.Await(subject);
				break;
			case "Remove":
			case "Delete":
				await _remove.Await(subject);
				break;
		}
	}
}