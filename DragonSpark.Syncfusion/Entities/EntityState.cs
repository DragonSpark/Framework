using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Stop;
using System.Threading.Tasks;

namespace DragonSpark.SyncfusionRendering.Entities;

public class EntityState<T> : IStopAware<Updated<T>> where T : class
{
	readonly IStopAware<T> _update;
	readonly IStopAware<T> _remove;

	protected EntityState(IStopAware<T> update, IStopAware<T> remove)
	{
		_update = update;
		_remove = remove;
	}

	public async ValueTask Get(Stop<Updated<T>> parameter)
	{
		var ((subject, action), stop) = parameter;
		switch (action)
		{
			case "Add":
			case "Edit":
				await _update.Off(new(subject, stop));
				break;
			case "Remove":
			case "Delete":
				await _remove.Off(new(subject, stop));
				break;
		}
	}
}