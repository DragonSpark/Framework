using DragonSpark.Compose;
using DragonSpark.Model.Operations;
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

	public async ValueTask<IStorageEntry> Get(Stop<DestinationInput> parameter)
	{
		var ((source, _), stop) = parameter;
		var result = await _copy.Off(parameter);
		await _delete.Off(new(source.Properties.Path, stop));
		return result;
	}
}