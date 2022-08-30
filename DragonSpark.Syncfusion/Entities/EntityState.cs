using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.SyncfusionRendering.Entities;

public class EntityState<T> : IOperation<Updated<T>> where T : class
{
	readonly IOperation<T> _update;
	readonly IOperation<T> _remove;

	protected EntityState(IOperation<T> update, IOperation<T> remove)
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