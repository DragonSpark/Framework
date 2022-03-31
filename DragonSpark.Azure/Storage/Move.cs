using DragonSpark.Compose;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Storage;

sealed class Move : IMove
{
	readonly ICopy   _copy;
	readonly IDelete _delete;

	public Move(ICopy copy, IDelete delete)
	{
		_copy   = copy;
		_delete = delete;
	}

	public async ValueTask<IStorageEntry> Get(DestinationInput parameter)
	{
		var (source, _) = parameter;
		var result = await _copy.Await(parameter);
		await _delete.Await(source.Properties.Name);
		return result;
	}
}