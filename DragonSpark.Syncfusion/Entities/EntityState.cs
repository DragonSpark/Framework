using DragonSpark.Application.Entities.Editing;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.SyncfusionRendering.Entities;

public class EntityState<T> : IOperation<Updated<T>> where T : class
{
	readonly Update<T> _update;
	readonly Remove<T> _remove;

	protected EntityState(Update<T> update, Remove<T> remove)
	{
		_update = update;
		_remove = remove;
	}

	public async ValueTask Get(Updated<T> parameter)
	{
		var (subject, action) = parameter;
		switch (action)
		{
			case "Add":
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